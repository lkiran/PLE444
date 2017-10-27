using PLE444.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using PLE444.ViewModels;
using PLE.Website.Service;

namespace PLE444.Controllers
{
	public class CommunityController : Controller
	{
		private PleDbContext db = new PleDbContext();
		private CommunityService _communityService;

		public CommunityController() {
			_communityService = new CommunityService();
		}

		public ActionResult Index(Guid? id) {
			if (id == null)
				return RedirectToAction("List", "Community");

			var community = db.Communities.Include("Owner").FirstOrDefault(c => c.Id == id);
			var model = new CommunityViewModel {
				Community = community,
				Status = Status(community),
				MemberCount = db.UserCommunities
					.Where(c => c.Community.Id == community.Id)
					.Count(u => u.DateJoined != null && u.IsActive)
			};

			return View(model);
		}

		public ActionResult List() {
			var model = db.Communities.Where(c => c.IsActive && !c.IsHiden).ToList();
			return View(model);
		}

		[ChildActionOnly]
		public ActionResult Navigation(Guid? id) {
			var model = db.Communities.SingleOrDefault(i => i.Id == id);
			return PartialView(model);
		}

		public ActionResult Create() {
			ViewBag.Sapces = db.Spaces.ToList();
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(Community community) {
			if (ModelState.IsValid) {
				var currentuserId = User.GetPrincipal()?.User.Id;

				community.OwnerId = currentuserId;
				community.DateCreated = DateTime.Now;

				db.Communities.Add(community);
				db.SaveChanges();
				return RedirectToAction("Index");
			}
			ViewBag.Sapces = db.Spaces.ToList();
			return View(community);
		}

		public ActionResult Edit(Guid id) {
			ViewBag.Sapces = db.Spaces.ToList();
			var currentuserId = User.GetPrincipal()?.User.Id;
			var c = db.Communities.Find(id);

			if (c.OwnerId == currentuserId)
				return View(c);

			return RedirectToAction("Index");
		}

		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(Community model) {
			if (ModelState.IsValid) {
				var community = db.Communities.Find(model.Id);
				if (community == null)
					return HttpNotFound();

				if (Status(community) != Enums.StatusType.Creator)
					return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);

				community.Name = model.Name;
				community.Description = model.Description;
				community.IsHiden = model.IsHiden;
				community.IsOpen = model.IsOpen;
				community.SpaceId = community.SpaceId;

				db.Entry(community).State = EntityState.Modified;
				db.SaveChanges();

				return RedirectToAction("Index", new { id = model.Id });
			}
			ViewBag.Sapces = db.Spaces.ToList();
			return View(model);
		}

		[Authorize]
		public ActionResult Discussion(Guid? id) {
			var community =
				db.Communities.Include("Discussions")
					.Include("Discussions.Messages")
					.Include("Discussions.Messages.Sender")
					.Include("Discussions.Readings")
					.FirstOrDefault(i => i.Id == id);
			if (community == null)
				return HttpNotFound();

			if (Status(community) != Enums.StatusType.Creator && Status(community) != Enums.StatusType.Member)
				return RedirectToAction("Index", "Community", new { id = id });

			community.Discussions = community.Discussions.OrderBy(d => d.DateCreated).ToList();
			ViewBag.Role = Status(community) == Enums.StatusType.Creator ? "Creator" : "Member";
			ViewBag.CurrentUserId = User.GetPrincipal()?.User.Id;
			return View(community);
		}

		[HttpPost]
		[Authorize]
		public ActionResult Read(Guid? discussionId, Guid? communityId) {
			var c = db.Communities.Include("Discussions").Include("Discussions.Readings").FirstOrDefault(i => i.Id == communityId);
			if (c == null)
				return Json(new { success = false });

			var d = c.Discussions.FirstOrDefault(i => i.ID == discussionId);
			if (d == null)
				return Json(new { success = false });

			var currentUser = User.GetPrincipal()?.User.Id;
			var r = d.Readings.FirstOrDefault(u => u.UserId == currentUser);
			if (r == null) {
				r = new Discussion.Reading {
					UserId = currentUser,
					Date = DateTime.Now
				};
				d.Readings.Add(r);
			} else {
				r.Date = DateTime.Now;
			}

			db.Entry(c).State = EntityState.Modified;
			db.SaveChanges();

			return Json(new { success = true });
		}

		[Authorize]
		public ActionResult AddTitle(string id) {
			ViewBag.CommunityId = id;
			return View();
		}

		[HttpPost]
		[Authorize]
		[ValidateAntiForgeryToken]
		public ActionResult AddTitle(Discussion discussion, Guid? communityId) {
			if (ModelState.IsValid) {
				var d = new Discussion();
				d.DateCreated = DateTime.Now;
				d.CreatorId = User.GetPrincipal()?.User.Id;
				d.Topic = discussion.Topic;

				d = db.Discussions.Add(d);

				var c = db.Communities.Find(communityId);
				c.Discussions.Add(d);

				db.Entry(c).State = EntityState.Modified;

				db.SaveChanges();

				return RedirectToAction("Discussion", new { id = communityId });
			}

			return View();
		}

		[Authorize]
		public ActionResult RemoveTitle(Guid? discussionId, Guid? communityId) {
			if (communityId == null || discussionId == null)
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

			var community = db.Communities.FirstOrDefault(c => c.Id == communityId);
			if (community == null)
				return HttpNotFound();

			else if (Status(community) != Enums.StatusType.Creator)
				return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);

			var discussion = db.Discussions.FirstOrDefault(d => d.ID == discussionId);
			if (discussion == null)
				return HttpNotFound();

			db.Readings.RemoveRange(discussion.Readings);
			db.Messages.RemoveRange(discussion.Messages);
			db.Discussions.Remove(discussion);
			db.SaveChanges();

			return Redirect(HttpContext.Request.UrlReferrer.AbsoluteUri);
		}

		[HttpPost]
		[Authorize]
		[ValidateAntiForgeryToken]
		public ActionResult SendMessage(Message message, Guid communityId, Guid discussionId) {
			if (ModelState.IsValid) {
				var m = new Message();
				m.Content = message.Content;
				m.DateSent = DateTime.Now;
				m.SenderId = User.GetPrincipal()?.User.Id;

				db.Messages.Add(m);

				var d = db.Discussions.Find(discussionId);
				d.Messages.Add(m);

				db.Entry(d).State = EntityState.Modified;

				db.SaveChanges();

				TempData["Active"] = discussionId;
			}
			return Redirect(HttpContext.Request.UrlReferrer.AbsoluteUri);
		}

		[Authorize]
		public ActionResult RemoveMessage(Guid? messageId, Guid? communityId) {
			if (messageId == null)
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

			var message = db.Messages.FirstOrDefault(m => m.ID == messageId);
			if (message == null)
				return HttpNotFound();

			else if (Status(communityId) != Enums.StatusType.Creator && message.SenderId != User.GetPrincipal()?.User.Id)
				return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);

			db.Messages.Remove(message);
			db.SaveChanges();

			return Redirect(HttpContext.Request.UrlReferrer.AbsoluteUri);
		}

		[Authorize]
		public ActionResult Members(Guid? id) {
			if (id == null)
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

			var community = db.Communities.Find(id);
			if (community == null)
				return HttpNotFound();

			if (Status(community) != Enums.StatusType.Member && Status(community) != Enums.StatusType.Creator)
				return RedirectToAction("Index", "Community", new { id = community.Id });

			var model = new CommunityMembers {
				Members = db.UserCommunities.Include("User").Where(uc => uc.CommunityId == id).ToList(),
				Community = community,
				CanEdit = Status(community) == Enums.StatusType.Creator
			};

			return View(model);
		}

		[Authorize]
		public ActionResult Join(Guid? id) {
			var userId = User.GetPrincipal()?.User.Id;

			var community = db.Communities.Find(id);
			if (community == null)
				return HttpNotFound();

			var uc = db.UserCommunities.Where(u => u.UserId == userId).FirstOrDefault(c => c.Community.Id == id);

			if (uc == null) {
				uc = new UserCommunity {
					Community = community,
					UserId = userId
				};

				if (!community.IsOpen)
					uc.DateJoined = DateTime.Now;
				else
					uc.DateJoined = null;

				db.UserCommunities.Add(uc);

				db.SaveChanges();
			}

			return RedirectToAction("Index", new { id = id });
		}

		[Authorize]
		public ActionResult Approve(Guid? id) {
			if (id == null)
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

			var userCommunity = db.UserCommunities.Include("Community").FirstOrDefault(uc => uc.Id == id);
			if (userCommunity == null)
				return HttpNotFound();

			if (Status(userCommunity.Community) != Enums.StatusType.Creator)
				return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);

			userCommunity.DateJoined = DateTime.Now;

			db.Entry(userCommunity).State = EntityState.Modified;
			db.SaveChanges();

			return RedirectToAction("Index", "Community", new { id = userCommunity.CommunityId });
		}

		[Authorize]
		[HttpPost]
		public ActionResult Approve(List<Guid> list) {
			if (!list.Any())
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

			foreach (var i in list) {
				var userCommunity = db.UserCommunities.Include("Community").FirstOrDefault(uc => uc.Id == i);
				if (userCommunity == null)
					return HttpNotFound();

				if (Status(userCommunity.Community) != Enums.StatusType.Creator)
					return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);

				userCommunity.DateJoined = DateTime.Now;

				db.Entry(userCommunity).State = EntityState.Modified;
			}

			db.SaveChanges();

			return new HttpStatusCodeResult(HttpStatusCode.OK);
		}

		[Authorize]
		public ActionResult Leave(Guid? id) {
			var userId = User.GetPrincipal()?.User.Id;
			var uc = db.UserCommunities.Where(u => u.UserId == userId).FirstOrDefault(c => c.Community.Id == id);

			if (uc != null) {
				db.UserCommunities.Remove(uc);
				db.SaveChanges();
			}

			return RedirectToAction("Index", new { id = id });
		}

		private Enums.StatusType Status(Guid? communityId) {
			if (communityId == null)
				return Enums.StatusType.None;

			var community = db.Communities.Find(communityId);
			return Status(community);
		}

		private Enums.StatusType Status(Community community) {
			var userId = User.GetPrincipal()?.User.Id;
			if (community.OwnerId == userId)
				return Enums.StatusType.Creator;
			else {
				var cu =
					db.UserCommunities.Where(c => c.Community.Id == community.Id)
						.FirstOrDefault(u => u.UserId == userId);
				if (cu == null)
					return Enums.StatusType.None;
				else if (cu.DateJoined == null)
					return Enums.StatusType.Waiting;
				else if (cu.IsActive)
					return Enums.StatusType.Member;
				else
					return Enums.StatusType.Removed;
			}
		}
	}
}