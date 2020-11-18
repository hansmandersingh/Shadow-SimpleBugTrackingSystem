using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Shadow.BL;
using Shadow.Models;

namespace Shadow.Controllers
{
    [Authorize(Roles ="submitter")]
    public class SubmittersController : Controller
    {
        SubmitterBusinessLayer SubmitterBusinessLayer = new SubmitterBusinessLayer();

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
            SubmitterBusinessLayer.CreateTicket(title, User.Identity.GetUserId(), projectId, description, ticketTypeId, ticketPrioritiesId, ticketStatusId);
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
    }
}
