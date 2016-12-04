using Microsoft.AspNet.Identity;
using PLE444.Context;
using PLE444.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace PLE444.Controllers
{
    public class CommunityController : Controller
    {
        // GET: Community 
        private PleDbContext db = new PleDbContext();
        private ApplicationDbContext UserDB = new ApplicationDbContext();

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Community community, HttpPostedFileBase uploadFile)
        {       
            if (ModelState.IsValid)
            {
                var currentuserId = User.Identity.GetUserId();
                var imageFilePath = "";

                if (uploadFile != null && uploadFile.ContentLength > 0) //check length of bytes are greater then zero or not
                {
                    //for checking uploaded file is image or not
                    if (Path.GetExtension(uploadFile.FileName).ToLower() == ".jpg"
                        || Path.GetExtension(uploadFile.FileName).ToLower() == ".png"
                        || Path.GetExtension(uploadFile.FileName).ToLower() == ".gif"
                        || Path.GetExtension(uploadFile.FileName).ToLower() == ".jpeg")
                    {
                        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(uploadFile.FileName);
                        imageFilePath = Path.Combine(Server.MapPath("~/Uploads"), fileName);
                        uploadFile.SaveAs(imageFilePath);
                        ViewBag.UploadSuccess = true;
                    }
                    community.GroupPhoto = "/Uploads/" + uploadFile.FileName;
                }

                community.AdminId = currentuserId;               
                db.Communities.Add(community);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(community);
        }

        public ActionResult Index(Guid? id)
        {
            if (id == null)
            {                
                var com = db.Communities.ToList();
                return View("List", com);
            }
            ViewBag.CurrentUserId = User.Identity.GetUserId();
            var c = db.Communities.Find(id);
            return View(c);
        }
       
        public ActionResult Events()
        {
            return View();
        }

        public ActionResult Discussion(Guid? id)
        {    
            var community = db.Communities.Find(id);                  
            var m = db.Communities.Include("Discussion").Include("Discussion.Messages").FirstOrDefault(i => i.ID == id);

                ViewBag.Active = TempData["Active"];
            ViewBag.CurrentUserId = User.Identity.GetUserId();
            
            return View(m);
        }

        public ActionResult Members(Guid? id)
        {
            var currentuserId = User.Identity.GetUserId();
            var c = db.Communities.Find(id);
            var m = db.UserCommunities.Where(i => i.Community.ID == id);
            var data = new CommunityMembersViewModel();
            data.CommunityInfo = c;
            data.Users = new List<UserViewModel>();
            var friends = db.Friendship.Where(o => o.userID == currentuserId).ToList();
            foreach (var item in m)
            {                
                var dbUser = UserDB.Users.Find(item.UserId);
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
            var uc = db.UserCommunities.Where(u => u.UserId == userId).FirstOrDefault(c => c.Community.ID == id);

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
                c.Discussion.Add(d);

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

        public ActionResult Edit(Guid id)
        {
            var currentuserId = User.Identity.GetUserId();
            var c = db.Communities.Find(id);

            if (c.AdminId == currentuserId)
                return View(c);         

            return RedirectToAction("Index");
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Community model, HttpPostedFileBase uploadFile)
        {
            if (ModelState.IsValid )
            {
                var imageFilePath = "";
                var fileName = "";

                if (uploadFile != null && uploadFile.ContentLength > 0) 
                {
                    if (Path.GetExtension(uploadFile.FileName).ToLower() == ".jpg"
                        || Path.GetExtension(uploadFile.FileName).ToLower() == ".png"
                        || Path.GetExtension(uploadFile.FileName).ToLower() == ".gif"
                        || Path.GetExtension(uploadFile.FileName).ToLower() == ".jpeg")
                    {
                        fileName = Guid.NewGuid().ToString() + Path.GetExtension(uploadFile.FileName);
                        imageFilePath = Path.Combine(Server.MapPath("~/Uploads"), fileName);
                        uploadFile.SaveAs(imageFilePath);
                        ViewBag.UploadSuccess = true;

                        model.GroupPhoto = "/Uploads/" + fileName;
                    }
                }


                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index", new { id = model.ID });
            }

            return View(model);
        }
    }
}