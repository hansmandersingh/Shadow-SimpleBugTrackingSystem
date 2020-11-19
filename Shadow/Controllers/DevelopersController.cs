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
    [Authorize(Roles = "developer")]
    public class DevelopersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        DeveloperBusinessLayer DeveloperBusinessLayer = new DeveloperBusinessLayer();
        // GET: Developers
        public ActionResult Index()
        {
            return View(db.ApplicationUsers.ToList());
        }

        // GET: Developers/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Developer developer = db.ApplicationUsers.Find(id);
            if (developer == null)
            {
                return HttpNotFound();
            }
            return View(developer);
        }

        // GET: Developers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Developers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName")] Developer developer)
        {
            if (ModelState.IsValid)
            {
                db.ApplicationUsers.Add(developer);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(developer);
        }

        // GET: Developers/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Developer developer = db.ApplicationUsers.Find(id);
            if (developer == null)
            {
                return HttpNotFound();
            }
            return View(developer);
        }

        // POST: Developers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName")] Developer developer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(developer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(developer);
        }

        // GET: Developers/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Developer developer = db.ApplicationUsers.Find(id);
            if (developer == null)
            {
                return HttpNotFound();
            }
            return View(developer);
        }

        // POST: Developers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Developer developer = db.ApplicationUsers.Find(id);
            db.ApplicationUsers.Remove(developer);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult AllProjects()
        {
            var projects = DeveloperBusinessLayer.AllProject(User.Identity.GetUserId());
            return View(projects);
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
