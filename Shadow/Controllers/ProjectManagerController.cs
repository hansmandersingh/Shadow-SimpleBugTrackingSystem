using Microsoft.AspNet.Identity;
using Shadow.BL;
using Shadow.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;

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

        public ActionResult GetAllTickets(string sortOrder,string currentFilter, string searchString, int? page)
        {
            List<Ticket> AllTickets;
            ViewBag.CurrentSort = sortOrder;

            if (searchString != null)
            {
                page = 1;
            } else
            {
                searchString = currentFilter;
            }
            switch (sortOrder)
            {
                case "OrderByAscending":
                    AllTickets = ProjectManagerBusinessLayer.GetAllTickets(User.Identity.GetUserId()).OrderBy(a => a.Title).ToList();
                    break;
                case "OrderByDescending":
                    AllTickets = ProjectManagerBusinessLayer.GetAllTickets(User.Identity.GetUserId()).OrderByDescending(d => d.Title).ToList();
                    break;
                default:
                    AllTickets = ProjectManagerBusinessLayer.GetAllTickets(User.Identity.GetUserId());
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

        public ActionResult EditTicket(int ticketId)
        {
            var ticket = ProjectManagerBusinessLayer.GetTicket(ticketId);

            ViewBag.TicketStatusList = ProjectManagerBusinessLayer.TicketStatuses();
            ViewBag.TicketPrioritiesList = ProjectManagerBusinessLayer.TicketPriorities();
            ViewBag.TicketTypeList = ProjectManagerBusinessLayer.TicketTypes();
            return View(ticket);
        }
        [HttpPost]
        public ActionResult EditTicket(int ticketId, string title, string description, int ticketStatusId, int ticketPrioritieId, int ticketTypeId)
        {
            var sendTicket = ProjectManagerBusinessLayer.GetTicket(ticketId);
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

            var result = ProjectManagerBusinessLayer.EditTicket(User.Identity.GetUserId(), ticket);

            ViewBag.TicketStatusList = ProjectManagerBusinessLayer.TicketStatuses();
            ViewBag.TicketPrioritiesList = ProjectManagerBusinessLayer.TicketPriorities();
            ViewBag.TicketTypeList = ProjectManagerBusinessLayer.TicketTypes();

            if (result)
                return RedirectToAction("GetAllTickets");
            else
                return RedirectToAction("Index");
        }

        public ActionResult AssignTicketToDeveloper(int ticketId)
        {
            ViewBag.DevelopersList = ProjectManagerBusinessLayer.GetAllUsers().Where(w => w.Roles.Any(a => a.RoleId == ProjectManagerBusinessLayer.GetRoleId("developer"))).ToList();
            ViewBag.ticketId = ticketId;
            return View();
        }

        [HttpPost]
        public ActionResult AssignTicketToDeveloper(int ticketId, string userId)
        {
            var result = ProjectManagerBusinessLayer.AssignToDeveloper(User.Identity.GetUserId(), ticketId, userId);

            ViewBag.DevelopersList = ProjectManagerBusinessLayer.GetAllUsers().Where(w => w.Roles.Any(a => a.RoleId == ProjectManagerBusinessLayer.GetRoleId("developer"))).ToList();
            ViewBag.ticketId = ticketId;
            if (result)
                return RedirectToAction("GetAllTickets");
            else
                return RedirectToAction("Index");
        }

        public ActionResult UnAssignTicket(int ticketId)
        {
            ProjectManagerBusinessLayer.UnAssignTicket(User.Identity.GetUserId(), ticketId);

            return RedirectToAction("GetAllTickets");
        }

        public ActionResult AddComment(int ticketId)
        {
            ViewBag.ticketId = ticketId;
            return View();
        }
        [HttpPost]
        public ActionResult AddComment(int ticketId, string commentText)
        {
            var result = ProjectManagerBusinessLayer.AddComments(User.Identity.GetUserId(), ticketId, commentText);

            if (result)
                return RedirectToAction("GetAllTickets");
            else
                return View(ticketId);
        }

        public ActionResult ShowAllComments(int ticketId)
        {
            return View(ProjectManagerBusinessLayer.AllComments(ticketId));
        }

        public ActionResult AddAttachment(int ticketId)
        {
            ViewBag.ticketId = ticketId;
            return View();
        }
        [HttpPost]
        public ActionResult AddAttachment(int ticketId, string fileUrl, string filePath, string description)
        {
            var result = ProjectManagerBusinessLayer.AddAttachment(User.Identity.GetUserId(), ticketId, fileUrl, filePath, description);

            if (result)
                return RedirectToAction("GetAllTickets");
            else
                return View(ticketId);
        }

        public ActionResult ViewAllAttachments(int ticketId)
        {
            return View(ProjectManagerBusinessLayer.ShowAllAttachments(ticketId));
        }

        public ActionResult TicketHistory(int ticketId)
        {
            return View(ProjectManagerBusinessLayer.FullHistory(ticketId));
        }
    }
}