using Microsoft.AspNet.Identity;
using Shadow.BL;
using Shadow.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Shadow.Controllers
{
    [Authorize(Roles ="project manager")]
    public class ProjectManagerController : Controller
    {
        ProjectManagerBusinessLayer ProjectManagerBusinessLayer = new ProjectManagerBusinessLayer();
        // GET: ProjectManager
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CreateAProject()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CreateAProject(string projectName)
        {
            var result = ProjectManagerBusinessLayer.AddANewProject(User.Identity.GetUserId(), projectName);
            if (result)
                return View();
            else
                return RedirectToAction("Index");
        }

        public ActionResult EditProject(int projectId)
        {
            var project = ProjectManagerBusinessLayer.GetProject(projectId);
            return View(project);
        }
        [HttpPost]
        public ActionResult EditProject(int projectId, Project project)
        {
            var result = ProjectManagerBusinessLayer.EditProject(User.Identity.GetUserId(), project);

            if (result)
                return View(project);
            else
                return RedirectToAction("Index");
        }

        public ActionResult AllProjectsByUser()
        {
            var projects = ProjectManagerBusinessLayer.GetAllofMyProjects(User.Identity.GetUserId());
            return View(projects);
        }

        public ActionResult AllProjects()
        {
            var projects = ProjectManagerBusinessLayer.AllProject(User.Identity.GetUserId());
            return View(projects);
        }

        public ActionResult AssignToProject()
        {
            ViewBag.UserList = ProjectManagerBusinessLayer.GetAllUsers();
            ViewBag.ProjectList = ProjectManagerBusinessLayer.AllProject(User.Identity.GetUserId());
            return View();
        }

        [HttpPost]
        public ActionResult AssignToProject(string userId, int projectId)
        {
            var result = ProjectManagerBusinessLayer.AssignUserToAProject(User.Identity.GetUserId(), userId, projectId);

            ViewBag.UserList = ProjectManagerBusinessLayer.GetAllUsers();
            ViewBag.ProjectList = ProjectManagerBusinessLayer.AllProject(User.Identity.GetUserId());

            if (result)
                return View();
            else
                return RedirectToAction("Index");
        }

        public ActionResult UnAssignFromProject()
        {
            ViewBag.UserList = ProjectManagerBusinessLayer.GetAllUsers();
            ViewBag.ProjectList = ProjectManagerBusinessLayer.AllProject(User.Identity.GetUserId());
            return View();
        }

        [HttpPost]
        public ActionResult UnAssignFromProject(string userId, int projectId)
        {
            var result = ProjectManagerBusinessLayer.UnAssignUserFromProject(User.Identity.GetUserId(), userId, projectId);

            ViewBag.UserList = ProjectManagerBusinessLayer.GetAllUsers();
            ViewBag.ProjectList = ProjectManagerBusinessLayer.AllProject(User.Identity.GetUserId());

            if (result)
                return View();
            else
                return RedirectToAction("Index");
        }

        public ActionResult GetAllTickets()
        {
            return View(ProjectManagerBusinessLayer.GetAllTickets(User.Identity.GetUserId()));
        }

        public ActionResult EditTicket()
        {
            ViewBag.TicketStatusList = ProjectManagerBusinessLayer.TicketStatuses();
            ViewBag.TicketPrioritiesList = ProjectManagerBusinessLayer.TicketPriorities();
            ViewBag.TicketTypeList = ProjectManagerBusinessLayer.TicketTypes();
            return View();
        }
        [HttpPost]
        public ActionResult EditTicket(string title, string description, int ticketStatusId, int ticketPrioritieId, int ticketTypeId)
        {
            Ticket ticket = new Ticket()
            {
                Title = title,
                Description = description,
                TicketStatusId = ticketStatusId,
                TicketPrioritieId = ticketPrioritieId,
                TicketTypeId = ticketTypeId,
                Updated = DateTime.Now,
            };

            var result = ProjectManagerBusinessLayer.EditTicket(User.Identity.GetUserId(), ticket);

            ViewBag.TicketStatusList = ProjectManagerBusinessLayer.TicketStatuses();
            ViewBag.TicketPrioritiesList = ProjectManagerBusinessLayer.TicketPriorities();
            ViewBag.TicketTypeList = ProjectManagerBusinessLayer.TicketTypes();

            if (result)
                return View();
            else
                return RedirectToAction("Index");
        }
    }
}