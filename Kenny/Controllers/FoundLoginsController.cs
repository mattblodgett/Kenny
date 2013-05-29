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
	public class FoundLoginsController : Controller
	{
		private KennyContext db = new KennyContext();

		//
		// POST: /FoundLogins/Validate/5

		[HttpPost]
		public ActionResult Validate(int id)
		{
			FoundLogin foundLogin = db.FoundLogins.Where(l => l.Id == id).Include(l => l.Site).First();
			foundLogin.IsValid = true;
			db.SaveChanges();
			return Redirect(Request.UrlReferrer.ToString());
		}

		//
		// POST: /FoundLogins/Invalidate/5

		[HttpPost]
		public ActionResult Invalidate(int id)
		{
			FoundLogin foundLogin = db.FoundLogins.Where(l => l.Id == id).Include(l => l.Site).First();
			foundLogin.IsValid = false;
			db.SaveChanges();
			return Redirect(Request.UrlReferrer.ToString());
		}

		//
		// POST: /FoundLogins/Collect/5

		[HttpPost]
		public ActionResult Collect(int siteId)
		{
			Site site = db.Sites.Find(siteId);
			FoundLoginCollector collector = new FoundLoginCollector();
			Dictionary<string, string> collectedLogins = collector.CollectLogins(site.AuthenticatedUrl);
			List<FoundLogin> currentLoginsForSite = db.FoundLogins.Where(l => l.Site.Id == siteId).ToList();
			foreach (var newLoginPair in collectedLogins)
			{
				if (!currentLoginsForSite.Any(l => l.Username == newLoginPair.Key))
				{
					FoundLogin newLogin = new FoundLogin();
					newLogin.Username = newLoginPair.Key;
					newLogin.Password = newLoginPair.Value;
					newLogin.Site = site;
					db.FoundLogins.Add(newLogin);
				}
			}
			db.SaveChanges();
			return RedirectToAction("Index", new { siteId = siteId });
		}

		//
		// GET: /FoundLogins/

		public ActionResult Index(int siteId = 0)
		{
			UserProfile currentUser = db.UserProfiles.FirstOrDefault(u => u.UserName == User.Identity.Name);
			Site site = db.Sites.Find(siteId);
			if (site == null || site.OwnerId != currentUser.UserId)
			{
				return HttpNotFound();
			}

			var foundLogins = new List<FoundLogin>();

			if (siteId > 0)
			{
				foundLogins = db.FoundLogins.Where(l => l.Site.Id == siteId && (!l.IsValid.HasValue || l.IsValid.Value)).Include(l => l.Site).ToList();
			}

			foundLogins = foundLogins.OrderByDescending(l => l.IsValid).ToList();

			return View(foundLogins);
		}

		protected override void Dispose(bool disposing)
		{
			db.Dispose();
			base.Dispose(disposing);
		}
	}
}