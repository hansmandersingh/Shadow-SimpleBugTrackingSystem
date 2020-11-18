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
    public class SubmittersController : Controller
    {
        SubmitterBusinessLayer SubmitterBusinessLayer = new SubmitterBusinessLayer();
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Submitters
        public ActionResult Index()
        {
            return View(db.ApplicationUsers.ToList());
        }

        // GET: Submitters/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Submitter submitter = db.ApplicationUsers.Find(id);
            if (submitter == null)
            {
                return HttpNotFound();
            }
            return View(submitter);
        }

        // GET: Submitters/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Submitters/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName")] Submitter submitter)
        {
            if (ModelState.IsValid)
            {
                db.ApplicationUsers.Add(submitter);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(submitter);
        }

        // GET: Submitters/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Submitter submitter = db.ApplicationUsers.Find(id);
            if (submitter == null)
            {
                return HttpNotFound();
            }
            return View(submitter);
        }

        // POST: Submitters/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName")] Submitter submitter)
        {
            if (ModelState.IsValid)
            {
                db.Entry(submitter).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(submitter);
        }

        // GET: Submitters/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Submitter submitter = db.ApplicationUsers.Find(id);
            if (submitter == null)
            {
                return HttpNotFound();
            }
            return View(submitter);
        }

        // POST: Submitters/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Submitter submitter = db.ApplicationUsers.Find(id);
            db.ApplicationUsers.Remove(submitter);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

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
    }
}
