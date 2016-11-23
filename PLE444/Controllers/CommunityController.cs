using Microsoft.AspNet.Identity;
using PLE444.Context;
using PLE444.Models;
using System;
using System.Collections.Generic;
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
            //JobPost = _db.JobPosts.Include(i => i.JobTags).First(i => i.Id == id),
            var m = db.Communities.Include("Discussion").Include("Discussion.Messages").FirstOrDefault(i => i.ID == id);
            return View(m);
        }
        public ActionResult Archive()
        {
            return View();
        }

    }
}