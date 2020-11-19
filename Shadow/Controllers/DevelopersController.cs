using System;
using System.Collections.Generic;
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
        public ActionResult GetAllTickets()
        {
            return View(DeveloperBusinessLayer.GetAllTickets(User.Identity.GetUserId()));
        }
        public ActionResult GetAllTicketAssignToDeveloper()
        {
            return View(DeveloperBusinessLayer.ticketsAssignToDeveloper(User.Identity.GetUserId()));
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
