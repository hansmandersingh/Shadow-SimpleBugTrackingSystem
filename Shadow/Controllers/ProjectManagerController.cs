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
        AdminBusinessLayer AdminBusinessLayer = new AdminBusinessLayer();
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
    }
}