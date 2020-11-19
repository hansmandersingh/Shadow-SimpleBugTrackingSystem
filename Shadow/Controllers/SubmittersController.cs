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
            var allProjects = SubmitterBusinessLayer.AllProject(User.Identity.GetUserId());
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
    }
}
