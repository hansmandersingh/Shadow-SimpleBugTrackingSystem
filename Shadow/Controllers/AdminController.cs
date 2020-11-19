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
    }
}