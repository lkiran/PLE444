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

                if (uploadFile.ContentLength > 0) //check length of bytes are greater then zero or not
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
                }

                community.AdminId = currentuserId;
                community.GroupPhoto = "/Uploads/" + uploadFile.FileName;
                db.Communities.Add(community);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(community);
        }

        public ActionResult Index(Guid? id)
        {
            if(id== null)
            {
                var com = db.Communities.ToList();
                return View("List",com);
            }
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

            if (TempData["Active"] == null)
                ViewBag.Active = m.Discussion.First().ID;
            else
                ViewBag.Active = TempData["Active"];
            ViewBag.CurrentUserId = User.Identity.GetUserId();
            
            return View(m);
        }

        public  ActionResult AddTitle(string id)
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

    }
}