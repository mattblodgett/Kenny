using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kenny.Models;

namespace Kenny.Controllers
{
    public class SitesController : Controller
    {
		private KennyContext db = new KennyContext();

        //
        // GET: /Sites/

        public ActionResult Index()
        {
			UserProfile currentUser = db.UserProfiles.FirstOrDefault(u => u.UserName == User.Identity.Name);

            return View(currentUser.Sites.ToList());
        }

        //
        // GET: /Sites/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Sites/Create

        [HttpPost]
        public ActionResult Create(Site site)
        {
			UserProfile currentUser = db.UserProfiles.FirstOrDefault(u => u.UserName == User.Identity.Name);

            if (ModelState.IsValid && currentUser != null)
            {
				site.Owner = currentUser;
				db.Sites.Add(site);
				db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(site);
        }

        //
        // GET: /Sites/Edit/5

        public ActionResult Edit(int id = 0)
        {
			UserProfile currentUser = db.UserProfiles.FirstOrDefault(u => u.UserName == User.Identity.Name);
			Site site = db.Sites.Find(id);
			if (site == null || site.OwnerId != currentUser.UserId)
			{
				return HttpNotFound();
			}
			return View(site);
        }

        //
        // POST: /Sites/Edit/5

        [HttpPost]
        public ActionResult Edit(Site site)
        {
            if (ModelState.IsValid)
            {
                db.Entry(site).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(site);
        }

        //
        // GET: /Sites/Delete/5

        public ActionResult Delete(int id = 0)
        {
			UserProfile currentUser = db.UserProfiles.FirstOrDefault(u => u.UserName == User.Identity.Name);
			Site site = db.Sites.Find(id);
			if (site == null || site.OwnerId != currentUser.UserId)
			{
				return HttpNotFound();
			}
			return View(site);
        }

        //
        // POST: /Sites/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Site site = db.Sites.Find(id);
            db.Sites.Remove(site);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}