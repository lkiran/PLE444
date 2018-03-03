using PLE444.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using PLE444.ViewModels;
using PLE.Website.Service;

namespace PLE444.Controllers
{
	public class CommunityController : Controller
	{
		#region Fields
		private PleDbContext db = new PleDbContext();
		private CommunityService _communityService;
		#endregion

		#region Ctor
		public CommunityController() {
			_communityService = new CommunityService();
		}
		#endregion

		#region Community
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
			return View(community);
		}

		public ActionResult Edit(Guid id) {
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

				db.Entry(community).State = EntityState.Modified;
				db.SaveChanges();

				return RedirectToAction("Index", new { id = model.Id });
			}
			return View(model);
		}

		#endregion

		#region Discussions
		[PleAuthorization]
		public ActionResult Discussion(Guid? id) {
			var community = db.Communities
				.Include("Discussions")
				.Include("Discussions.Messages")
				.Include("Discussions.Messages.Sender")
				.Include("Discussions.Readings")
				.FirstOrDefault(i => i.Id == id);

			if (community == null)
				return HttpNotFound();

			if (Status(community) != Enums.StatusType.Creator && Status(community) != Enums.StatusType.Member)
				return RedirectToAction("Index", "Community", new { id = id });

			var model = new DiscussionViewModel {
				CId = community.Id,
				CurrentUserId = User.Identity?.GetUserId(),
				Role = Status(community) == Enums.StatusType.Creator ? "Creator" : "Member",
				Discussion = community.Discussions.ToList()
			};

			return View(model);
		}

		#region Title
		[PleAuthorization]
		public ActionResult AddTitle(string id) {
			ViewBag.CommunityId = id;
			return View();
		}

		[HttpPost]
		[PleAuthorization]
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

		[PleAuthorization]
		public ActionResult RemoveTitle(Guid? discussionId, Guid? CId) {
			if (CId == null || discussionId == null)
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

			var community = db.Communities.FirstOrDefault(c => c.Id == CId);
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
		#endregion

		#region Messages
		[HttpPost]
		[PleAuthorization]
		public ActionResult Read(Guid? discussionId, Guid? CId) {
			if (discussionId == Guid.Empty || CId == Guid.Empty)
				return HttpNotFound();
			var c = db.Communities.Include("Discussions")
				.Include("Discussions.Messages.Sender")
				.Include("Discussions.Messages.Replies.Sender")
				.Include("Discussions.Readings")
				.FirstOrDefault(i => i.Id == CId);
			if (c == null)
				return Json(new { success = false });

			var d = c.Discussions.FirstOrDefault(i => i.ID == discussionId);
			if (d == null)
				return Json(new { success = false });

			var currentUser = User.Identity.GetUserId();
			var r = d.Readings.FirstOrDefault(u => u.UserId == currentUser);
			if (r == null) {
				r = new Discussion.Reading {
					UserId = currentUser,
					Date = DateTime.Now
				};
				d.Readings.Add(r);
			}
			else {
				r.Date = DateTime.Now;
			}

			db.Entry(c).State = EntityState.Modified;
			db.SaveChanges();
			var model = new DiscussionMessages {
				Discussion = d,
				CurrentUserId = currentUser,
				CId = (Guid)CId,
				Role = Status(c).ToString()
			};
			return PartialView(model);
		}

		[HttpPost]
		[PleAuthorization]
		[ValidateAntiForgeryToken]
		public ActionResult SendMessage(NewMessageViewModel model) {
			if (!ModelState.IsValid)
				return View(model);

			var newMessage = new Message {
				Content = model.Content,
				DateSent = DateTime.Now,
				SenderId = User.Identity.GetUserId()
			};

			if (model.ReplyId != Guid.Empty) {
				var parentMessage = db.Messages.Find(model.ReplyId);
				parentMessage?.Replies.Add(newMessage);

				db.Entry(parentMessage).State = EntityState.Modified;
			}
			else {
				db.Messages.Add(newMessage);

				var discussion = db.Discussions.Find(model.DiscussionId);
				discussion?.Messages.Add(newMessage);

				db.Entry(discussion).State = EntityState.Modified;
			}

			db.SaveChanges();

			TempData["Active"] = model.DiscussionId;
			return RedirectToAction("Discussion", new { id = model.CId });
		}


		[PleAuthorization]
		public ActionResult RemoveMessage(Guid? messageId, Guid? CId) {
			if (messageId == null)
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

			try {
				var message = db.Messages.Include("Replies").FirstOrDefault(m => m.ID == messageId);
				if (message == null)
					return HttpNotFound();

				if (Status(CId) != Enums.StatusType.Creator && message.SenderId != User.GetPrincipal()?.User.Id)
					return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);

				foreach (var reply in message.Replies.ToList())
					db.Messages.Remove(reply);


				db.Messages.Remove(message);
				db.SaveChanges();
			}
			catch (Exception e) { }

			return Redirect(HttpContext.Request.UrlReferrer.AbsoluteUri);
		}
		#endregion

		#endregion

		#region Members
		[PleAuthorization]
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

		[PleAuthorization]
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

		[PleAuthorization]
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

		[PleAuthorization]
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

		[PleAuthorization]
		public ActionResult Leave(Guid? id) {
			var userId = User.GetPrincipal()?.User.Id;
			var uc = db.UserCommunities.Where(u => u.UserId == userId).FirstOrDefault(c => c.Community.Id == id);

			if (uc != null) {
				db.UserCommunities.Remove(uc);
				db.SaveChanges();
			}

			return RedirectToAction("Index", new { id = id });
		}

		#endregion

		#region Private Methods
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
		#endregion
	}
}