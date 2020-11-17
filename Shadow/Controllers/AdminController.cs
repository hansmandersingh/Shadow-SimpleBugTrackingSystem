using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Shadow.BL;

namespace Shadow.Controllers
{
    [Authorize]
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
    }
}