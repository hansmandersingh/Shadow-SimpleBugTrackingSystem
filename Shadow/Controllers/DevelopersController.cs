using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using PagedList;
using Shadow.BL;
using Shadow.Models;

namespace Shadow.Controllers
{
    [Authorize(Roles = "developer")]
    public class DevelopersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        DeveloperBusinessLayer DeveloperBusinessLayer = new DeveloperBusinessLayer();

        public ActionResult AllProject()
        {
            var projects = DeveloperBusinessLayer.AllProject(User.Identity.GetUserId());
            return View(projects);
        }
        public ActionResult GetAllTickets(string sortOrder, string currentFilter, string searchString, int? page)
        {
            List<Ticket> AllTickets;
            ViewBag.CurrentSort = sortOrder;

            if (sortOrder != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            switch (sortOrder)
            {
                case "OrderByAscending":
                    AllTickets = DeveloperBusinessLayer.GetAllTickets(User.Identity.GetUserId()).OrderBy(a => a.Title).ToList();
                    break;
                case "OrderByDescending":
                    AllTickets = DeveloperBusinessLayer.GetAllTickets(User.Identity.GetUserId()).OrderByDescending(d => d.Title).ToList();
                    break;
                default:
                    AllTickets = DeveloperBusinessLayer.GetAllTickets(User.Identity.GetUserId());
                    break;
            }

            if (!string.IsNullOrEmpty(searchString))
            {
                AllTickets = AllTickets.Where(s => s.Title.Contains(searchString) || s.Description.Contains(searchString)).ToList();
            }

            int pageSize = 5;
            int pageNumber = (page ?? 1);

            return View(AllTickets.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult GetAllTicketAssignToDeveloper(string sortOrder, string currentFilter, string searchString, int? page)
        {
            List<Ticket> AllTickets;
            ViewBag.CurrentSort = sortOrder;

            if (sortOrder != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            switch (sortOrder)
            {
                case "OrderByAscending":
                    AllTickets = DeveloperBusinessLayer.ticketsAssignToDeveloper(User.Identity.GetUserId()).OrderBy(a => a.Title).ToList();
                    break;
                case "OrderByDescending":
                    AllTickets = DeveloperBusinessLayer.ticketsAssignToDeveloper(User.Identity.GetUserId()).OrderByDescending(d => d.Title).ToList();
                    break;
                default:
                    AllTickets = DeveloperBusinessLayer.ticketsAssignToDeveloper(User.Identity.GetUserId());
                    break;
            }

            if (!string.IsNullOrEmpty(searchString))
            {
                AllTickets = AllTickets.Where(s => s.Title.Contains(searchString) || s.Description.Contains(searchString)).ToList();
            }

            int pageSize = 5;
            int pageNumber = (page ?? 1);

            return View(AllTickets.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult AddComment(int ticketId)
        {
            ViewBag.ticketId = ticketId;
            return View();
        }
        [HttpPost]
        public ActionResult AddComment(int ticketId, string commentText)
        {
            var result = DeveloperBusinessLayer.AddComments(User.Identity.GetUserId(), ticketId, commentText);

            if (result)
                return RedirectToAction("GetAllTickets");
            else
                return View(ticketId);
        }

        public ActionResult ShowAllComments(int ticketId)
        {
            return View(DeveloperBusinessLayer.AllComments(ticketId));
        }
        public ActionResult AddAttachment(int ticketId)
        {
            ViewBag.ticketId = ticketId;
            return View();
        }
        [HttpPost]
        public ActionResult AddAttachment(int ticketId, string fileUrl, string filePath, string description)
        {
            var result = DeveloperBusinessLayer.AddAttachment(User.Identity.GetUserId(), ticketId, fileUrl, filePath, description);

            if (result)
                return RedirectToAction("GetAllTickets");
            else
                return View(ticketId);
        }

        public ActionResult ViewAllAttachments(int ticketId)
        {
            return View(DeveloperBusinessLayer.ShowAllAttachments(ticketId));
        }
        public ActionResult TicketHistory(int ticketId)
        {
            return View(DeveloperBusinessLayer.FullHistory(ticketId));
        }
        public ActionResult AllNotification(int ticketId)
        {
            return View(DeveloperBusinessLayer.AllNotification(ticketId));
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

    }
}
