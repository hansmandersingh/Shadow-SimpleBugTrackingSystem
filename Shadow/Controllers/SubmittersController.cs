using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using PagedList;
using Shadow.BL;
using Shadow.DAL;
using Shadow.Models;

namespace Shadow.Controllers
{
    [Authorize(Roles ="submitter")]
    public class SubmittersController : Controller
    {
        SubmitterBusinessLayer SubmitterBusinessLayer = new SubmitterBusinessLayer();
        TicketRepository TicketRepository = new TicketRepository();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CreateTicket()
        {
            ViewBag.ProjectList = SubmitterBusinessLayer.ListProjects(User.Identity.GetUserId());
            ViewBag.TicketTypes = SubmitterBusinessLayer.TicketTypes();
            ViewBag.TicketPriorities = SubmitterBusinessLayer.AllTicketPriorities();
            ViewBag.TicketStatuses = SubmitterBusinessLayer.TicketStatuses();
            return View();
        }
        [HttpPost]
        public ActionResult CreateTicket(string title, int projectId, string description, int ticketTypeId, int ticketPrioritiesId, int ticketStatusId)
        {
            var userId = User.Identity.GetUserId();
            SubmitterBusinessLayer.CreateTicket(title, userId, projectId, description, ticketTypeId, ticketPrioritiesId, ticketStatusId);
            ViewBag.ProjectList = SubmitterBusinessLayer.ListProjects(User.Identity.GetUserId());
            ViewBag.TicketTypes = SubmitterBusinessLayer.TicketTypes();
            ViewBag.TicketPriorities = SubmitterBusinessLayer.AllTicketPriorities();
            ViewBag.TicketStatuses = SubmitterBusinessLayer.TicketStatuses();
            return View();
        }

        public ActionResult AllProjects()
        {
            var allProjects = SubmitterBusinessLayer.ListProjects(User.Identity.GetUserId());
            return View(allProjects);
        }

        public ActionResult GetAllTickets(string sortingOrder, string currentFilter, string searchString, int? page)
        {
            List<Ticket> Tickets;
            ViewBag.currentSort = sortingOrder;

            if(sortingOrder != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            switch (sortingOrder)
            {
                case "OrderByAscending":
                    Tickets = SubmitterBusinessLayer.GetAllTickets(User.Identity.GetUserId())
                        .OrderBy(t => t.Title).ToList();
                    break;
                case "OrderByDescending":
                    Tickets = SubmitterBusinessLayer.GetAllTickets(User.Identity.GetUserId())
                        .OrderByDescending(t => t.Title).ToList();
                    break;
                default:
                    Tickets = SubmitterBusinessLayer.GetAllTickets(User.Identity.GetUserId());
                    break;

            }
            if (!string.IsNullOrEmpty(searchString))
            {
                Tickets = Tickets.Where(s => s.Title.Contains(searchString) || s.Description.Contains(searchString)).ToList();
            }

            int pageSize = 5;
            int pageNumber = (page ?? 1);

            return View(Tickets.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult EditTicket(int ticketId)
        {
            var ticket = SubmitterBusinessLayer.GetTicket(ticketId);

            ViewBag.TicketStatusList = SubmitterBusinessLayer.TicketStatuses();
            ViewBag.TicketPrioritiesList = SubmitterBusinessLayer.AllTicketPriorities();
            ViewBag.TicketTypeList = SubmitterBusinessLayer.TicketTypes();
            return View(ticket);
        }
        [HttpPost]
        public ActionResult EditTicket(int ticketId, string title, string description, int ticketStatusId, int ticketPrioritieId, int ticketTypeId)
        {
            var sendTicket = SubmitterBusinessLayer.GetTicket(ticketId);
            Ticket ticket = new Ticket()
            {
                Id = ticketId,
                Title = title,
                Description = description,
                TicketStatusId = ticketStatusId,
                TicketPrioritieId = ticketPrioritieId,
                TicketTypeId = ticketTypeId,
                Updated = DateTime.Now,
                Created = sendTicket.Created,
                ProjectId = sendTicket.ProjectId,
                OwnerId = sendTicket.OwnerId,
                AssignedToUserId = sendTicket.AssignedToUserId,
            };

            var result = SubmitterBusinessLayer.EditTicket(User.Identity.GetUserId(), ticket);

            ViewBag.TicketStatusList = SubmitterBusinessLayer.TicketStatuses();
            ViewBag.TicketPrioritiesList = SubmitterBusinessLayer.AllTicketPriorities();
            ViewBag.TicketTypeList = SubmitterBusinessLayer.TicketTypes();

            if (result)
                return RedirectToAction("GetAllTickets");
            else
                return RedirectToAction("Index");
        }

        public ActionResult AddComment(int ticketId)
        {
            ViewBag.ticketId = ticketId;
            return View();
        }
        [HttpPost]
        public ActionResult AddComment(int ticketId, string commentText)
        {
            var result = SubmitterBusinessLayer.AddComments(User.Identity.GetUserId(), ticketId, commentText);

            if (result)
                return RedirectToAction("GetAllTickets");
            else
                return View(ticketId);
        }

        public ActionResult ShowAllComments(int ticketId)
        {
            return View(SubmitterBusinessLayer.AllComments(ticketId));
        }

        public ActionResult AddAttachment(int ticketId)
        {
            ViewBag.ticketId = ticketId;
            return View();
        }
        [HttpPost]
        public ActionResult AddAttachment(int ticketId, string fileUrl, string filePath, string description)
        {
            var result = SubmitterBusinessLayer.AddAttachment(User.Identity.GetUserId(), ticketId, fileUrl, filePath, description);

            if (result)
                return RedirectToAction("GetAllTickets");
            else
                return View(ticketId);
        }

        public ActionResult ViewAllAttachments(int ticketId)
        {
            return View(SubmitterBusinessLayer.ShowAllAttachments(ticketId));
        }

        public ActionResult TicketHistory(int ticketId)
        {
            return View(SubmitterBusinessLayer.FullHistory(ticketId));
        }
    }
}
