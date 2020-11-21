using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Shadow.BL;
using Shadow.Models;
using PagedList;

namespace Shadow.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        AdminBusinessLayer AdminBusinessLayer = new AdminBusinessLayer();
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult AssignRoleToUser()
        {
            ViewBag.UsersList = AdminBusinessLayer.GetAllUsers();
            return View();
        }
        [HttpPost]
        public ActionResult AssignRoleToUser(string userId, string roleName)
        {
            var result = AdminBusinessLayer.AssignUserToRole(User.Identity.GetUserId(), userId, roleName);
            ViewBag.UsersList = AdminBusinessLayer.GetAllUsers();
            if (result)
            {
                return View();
            }
            return RedirectToAction("Index");
        }
        public ActionResult UnAssignRole()
        {
            ViewBag.UsersList = AdminBusinessLayer.GetAllUsers();
            return View();
        }
        [HttpPost]
        public ActionResult UnAssignRole(string userId, string roleName)
        {
            var result = AdminBusinessLayer.UnAssignUserFromRole(User.Identity.GetUserId(), userId, roleName);
            ViewBag.UsersList = AdminBusinessLayer.GetAllUsers();
            if (result)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        public ActionResult CreateAProject()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CreateAProject(string projectName)
        {
            var result = AdminBusinessLayer.AddANewProject(User.Identity.GetUserId(), projectName);
            if (result)
                return View();
            else
                return RedirectToAction("Index");
        }

        public ActionResult EditProject(int projectId)
        {
            var project = AdminBusinessLayer.GetProject(projectId);
            return View(project);
        }
        [HttpPost]
        public ActionResult EditProject(int projectId, Project project)
        {
            var result = AdminBusinessLayer.EditProject(User.Identity.GetUserId(), project);

            if (result)
                return View(project);
            else
                return RedirectToAction("Index");
        }

        public ActionResult AllProjectsByUser()
        {
            var projects = AdminBusinessLayer.GetAllofMyProjects(User.Identity.GetUserId());
            return View(projects);
        }

        public ActionResult AllProjects()
        {
            var projects = AdminBusinessLayer.AllProject(User.Identity.GetUserId());
            return View(projects);
        }

        public ActionResult AssignToProject()
        {
            ViewBag.UserList = AdminBusinessLayer.GetAllUsers();
            ViewBag.ProjectList = AdminBusinessLayer.AllProject(User.Identity.GetUserId());
            return View();
        }

        [HttpPost]
        public ActionResult AssignToProject(string userId, int projectId)
        {
            var result = AdminBusinessLayer.AssignUserToAProject(User.Identity.GetUserId(), userId, projectId);

            ViewBag.UserList = AdminBusinessLayer.GetAllUsers();
            ViewBag.ProjectList = AdminBusinessLayer.AllProject(User.Identity.GetUserId());

            if (result)
                return View();
            else
                return RedirectToAction("Index");
        }

        public ActionResult UnAssignFromProject()
        {
            ViewBag.UserList = AdminBusinessLayer.GetAllUsers();
            ViewBag.ProjectList = AdminBusinessLayer.AllProject(User.Identity.GetUserId());
            return View();
        }

        [HttpPost]
        public ActionResult UnAssignFromProject(string userId, int projectId)
        {
            var result = AdminBusinessLayer.UnAssignUserFromProject(User.Identity.GetUserId(), userId, projectId);

            ViewBag.UserList = AdminBusinessLayer.GetAllUsers();
            ViewBag.ProjectList = AdminBusinessLayer.AllProject(User.Identity.GetUserId());
            if (result)
                return View();
            else
                return RedirectToAction("Index");
        }

        public ActionResult GetAllTickets(string sortOrder,string currentFilter, string searchString, int? page) 
        {
            ViewBag.CurrentSort = sortOrder;
            List<Ticket> AllTickets;
            
            if(searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewBag.CurrentFilter = searchString;

            switch (sortOrder)
            {
                case "OrderByAscending":
                    AllTickets = AdminBusinessLayer.GetAllTickets().OrderBy(a => a.Title).ToList();
                    break;
                case "OrderByDescending":
                    AllTickets = AdminBusinessLayer.GetAllTickets().OrderByDescending(a => a.Title).ToList();
                    break;
                default:
                    AllTickets = AdminBusinessLayer.GetAllTickets().ToList();
                    break;
            }

            if (!String.IsNullOrEmpty(searchString))
            {
                AllTickets = AllTickets.Where(s => s.Title.Contains(searchString) || s.Description.Contains(searchString)).ToList();
            }
            int pageSize = 5;

            int pageNumber = (page ?? 1);

            return View(AllTickets.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult EditTicket(int ticketId)
        {
            var ticket = AdminBusinessLayer.GetTicket(ticketId);

            ViewBag.TicketStatusList = AdminBusinessLayer.TicketStatuses();
            ViewBag.TicketPrioritiesList = AdminBusinessLayer.TicketPriorities();
            ViewBag.TicketTypeList = AdminBusinessLayer.TicketTypes();
            return View(ticket);
        }
        [HttpPost]
        public ActionResult EditTicket(int ticketId, string title, string description, int ticketStatusId, int ticketPrioritieId, int ticketTypeId)
        {
            var sendTicket = AdminBusinessLayer.GetTicket(ticketId);
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

            var result = AdminBusinessLayer.EditTicket(User.Identity.GetUserId(), ticket);

            ViewBag.TicketStatusList = AdminBusinessLayer.TicketStatuses();
            ViewBag.TicketPrioritiesList = AdminBusinessLayer.TicketPriorities();
            ViewBag.TicketTypeList = AdminBusinessLayer.TicketTypes();

            if (result)
                return RedirectToAction("GetAllTickets");
            else
                return RedirectToAction("Index");
        }

        public ActionResult AssignTicketToUser(int ticketId)
        {
            ViewBag.DevelopersList = AdminBusinessLayer.GetAllUsers().ToList();
            ViewBag.ticketId = ticketId;
            return View();
        }

        [HttpPost]
        public ActionResult AssignTicketToUser(int ticketId, string userId)
        {
            var result = AdminBusinessLayer.AssignToDeveloper(User.Identity.GetUserId(), ticketId, userId);

            ViewBag.DevelopersList = AdminBusinessLayer.GetAllUsers().ToList();
            ViewBag.ticketId = ticketId;
            if (result)
                return RedirectToAction("GetAllTickets");
            else
                return RedirectToAction("Index");
        }

        public ActionResult UnAssignTicket(int ticketId)
        {
            AdminBusinessLayer.UnAssignTicket(User.Identity.GetUserId(), ticketId);

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
            var result = AdminBusinessLayer.AddCommentOnTicket(User.Identity.GetUserId(), ticketId, commentText);

            if (result)
                return RedirectToAction("GetAllTickets");
            else
                return View(ticketId);
        }

        public ActionResult AllComments(int ticketId)
        {
            return View(AdminBusinessLayer.AllComments(ticketId));
        }

        public ActionResult AddAttachment(int ticketId)
        {
            ViewBag.ticketId = ticketId;
            return View();
        }
        [HttpPost]
        public ActionResult AddAttachment(int ticketId, string fileUrl, string filePath, string description)
        {
            var result = AdminBusinessLayer.AddAttachment(User.Identity.GetUserId(), ticketId, fileUrl, filePath, description);

            if (result)
                return RedirectToAction("GetAllTickets");
            else
                return View(ticketId);
        }

        public ActionResult ViewAllAttachments(int ticketId)
        {
            return View(AdminBusinessLayer.ShowAllAttachments(ticketId));
        }

        public ActionResult TicketHistory(int ticketId)
        {
            return View(AdminBusinessLayer.FullHistory(ticketId));
        }


    }
}