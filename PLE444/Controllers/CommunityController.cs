using Microsoft.AspNet.Identity;
using PLE444.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PLE444.ViewModels;

namespace PLE444.Controllers
{
    public class CommunityController : Controller
    {
        private PleDbContext db = new PleDbContext();

        public ActionResult Index(Guid? id)
        {
            if (id == null)
                return RedirectToAction("List", "Community");

            var community = db.Communities.Include("Owner").FirstOrDefault(c => c.Id == id);
            var model = new CommunityViewModel
            {
                Community = community,
                Status = Status(community),
                MemberCount = db.UserCommunities
                    .Where(c => c.Community.Id == community.Id)
                    .Count(u => u.DateJoined != null && u.IsActive)
            };

            return View(model);
        }

        public ActionResult List()
        {
            var model = db.Communities.Where(c=>c.IsActive && !c.IsHiden).ToList();
            return View(model);
        }

        [ChildActionOnly]
        public ActionResult Navigation(Guid? id)
        {
            var model = db.Communities.SingleOrDefault(i => i.Id == id);
            return PartialView(model);
        }

        public ActionResult Create()
        {
            ViewBag.Sapces = db.Spaces.ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Community community)
        {       
            if (ModelState.IsValid)
            {
                var currentuserId = User.Identity.GetUserId();

                community.OwnerId = currentuserId;
                community.DateCreated = DateTime.Now;
                              
                db.Communities.Add(community);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Sapces = db.Spaces.ToList();
            return View(community);
        }
       
        public ActionResult Edit(Guid id)
        {
            ViewBag.Sapces = db.Spaces.ToList();
            var currentuserId = User.Identity.GetUserId();
            var c = db.Communities.Find(id);

            if (c.OwnerId == currentuserId)
                return View(c);

            return RedirectToAction("Index");
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Community model)
        {
            if (ModelState.IsValid)
            {
                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index", new { id = model.Id });
            }
            ViewBag.Sapces = db.Spaces.ToList();
            return View(model);
        }


        public ActionResult Discussion(Guid? id)
        {
            var community = db.Communities.Find(id);                  
            var m = db.Communities.Include("Discussion").Include("Discussion.Messages").Include("Discussion.Readings").FirstOrDefault(i => i.Id == id);

            ViewBag.CurrentUserId = User.Identity.GetUserId();
            
            return View(m);
        }

        [HttpPost]
        [Authorize]
        public ActionResult Read(Guid? DiscussionId, Guid? CommunityId)
        {
            var c = db.Communities.Include("Discussion").Include("Discussion.Readings").FirstOrDefault(i => i.Id == CommunityId);
            if(c == null)
                return Json(new { success = false });

            var d = c.Discussions.FirstOrDefault(i => i.ID == DiscussionId);
            if (d == null)
                return Json(new { success = false });

            var currentUser = User.Identity.GetUserId();
            var r = d.Readings.FirstOrDefault(u => u.UserId == currentUser);
            if(r == null)
            {
                r = new Discussion.Reading();
                r.UserId = currentUser;
                r.Date = DateTime.Now;
                d.Readings.Add(r);
            }
            else
            {
                r.Date = DateTime.Now;                
            }

            db.Entry(c).State = EntityState.Modified;
            db.SaveChanges();

            return Json(new { success = true });
        }

        public ActionResult Members(Guid? id)
        {
            var currentuserId = User.Identity.GetUserId();
            var c = db.Communities.Find(id);
            var m = db.UserCommunities.Where(i => i.Community.Id == id);
            var data = new CommunityMembersViewModel();
            data.CommunityInfo = c;
            data.Users = new List<UserViewModel>();
            var friends = db.Friendship.Where(o => o.userID == currentuserId).ToList();
            foreach (var item in m)
            {                
                var dbUser = db.Users.Find(item.UserId);
                var userInfo = new UserViewModel();

                userInfo.UserID = dbUser.Id;
                userInfo.Name = dbUser.FirstName;
                userInfo.Surname = dbUser.LastName;
                userInfo.ProfilePhoto = dbUser.ProfilePicture;
                if(dbUser.Id != currentuserId)
                    userInfo.isFriend = friends.Any(o => o.FriendID == dbUser.Id);               
                    
                data.Users.Add(userInfo);
            }
            return View(data);
        }

        public ActionResult Join(Guid? id)
        {
            var userId = User.Identity.GetUserId();
            var uc = db.UserCommunities.Where(u => u.UserId == userId).FirstOrDefault(c => c.Community.Id == id);

            if(uc == null)
            {
                uc = new UserCommunity();
                uc.Community = db.Communities.Find(id);                
                uc.UserId = userId;
                uc.DateJoined = DateTime.Now;

                db.UserCommunities.Add(uc);

                db.SaveChanges();               
            }

            return RedirectToAction("Index", new { id = id });
        }

        public ActionResult Leave(Guid? id)
        {
            var userId = User.Identity.GetUserId();
            var uc = db.UserCommunities.Where(u => u.UserId == userId).FirstOrDefault(c => c.Community.Id == id);

            if(uc != null)
            {
                db.UserCommunities.Remove(uc);
                db.SaveChanges();
            }

            return RedirectToAction("Index", new { id = id });
        }

        public ActionResult AddTitle(string id)
        {
            ViewBag.CommunityId = id;           
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddTitle(Discussion discussion, Guid communityId)
        {
            if (ModelState.IsValid)
            {
                var d = new Discussion();
                d.DateCreated = DateTime.Now;
                d.CreatorId= User.Identity.GetUserId();
                d.Topic = discussion.Topic;

                db.Discussions.Add(d);

                var c = db.Communities.Find(communityId);
                c.Discussions.Add(d);

                db.Entry(c).State = EntityState.Modified;

                db.SaveChanges();

                return RedirectToAction("Discussion", new { id = communityId });
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SendMessage(Message message, Guid communityId, Guid discussionId)
        {
            if (ModelState.IsValid)
            {
                var m = new Message();
                m.Content = message.Content;
                m.DateSent = DateTime.Now;
                m.SenderId = User.Identity.GetUserId();

                db.Messages.Add(m);

                var d = db.Discussions.Find(discussionId);
                d.Messages.Add(m);

                db.Entry(d).State = EntityState.Modified;

                db.SaveChanges();

                TempData["Active"] = discussionId;
                return RedirectToAction("Discussion", new { id = communityId });
            }
            return View();
        }

        public ActionResult Archive()
        {
            return View();
        }

        private Enums.StatusType Status(Guid? communityId)
        {
            if (communityId == null)
                return Enums.StatusType.None;

            var community = db.Communities.Find(communityId);
            return Status(community);
        }

        private Enums.StatusType Status(Community community)
        {
            var userId = User.Identity.GetUserId();
            if (community.OwnerId == userId)
                return Enums.StatusType.Creator;
            else
            {
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