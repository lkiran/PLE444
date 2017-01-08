using Microsoft.AspNet.Identity;
using PLE444.Context;
using PLE444.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace PLE444.Controllers
{
    public class UserController : Controller
    {

        private ApplicationDbContext userdb = new ApplicationDbContext();
        private PleDbContext db = new PleDbContext();

        [Authorize]
        public ActionResult Profil(string id)
        {
            var currentUser = User.Identity.GetUserId();
            if (id == null)
            {
                id = currentUser;
            }

            ViewBag.isFriend = false;
            var fs = db.Friendship.Where(u => u.userID == currentUser).FirstOrDefault(f => f.FriendID == id);
            if (fs != null)
                ViewBag.isFriend = true;

            var userDetail = userdb.Users.Find(id);

            ViewBag.CurrentUser = currentUser;
            return View(userDetail);
        }

        [Authorize]
        public ActionResult MyFriends()
        {
            var activeUserId = User.Identity.GetUserId();
            var myfriends = db.Friendship.Where(m => m.userID == activeUserId).ToList();

            return View(myfriends);
        }

        public ActionResult AddFriend(String id)
        {
            var currentuserId = User.Identity.GetUserId();
            var fs = db.Friendship.Where(u => u.userID == currentuserId).FirstOrDefault(f => f.FriendID == id);

            if (fs == null)
            {
                var friends = userdb.Users.ToList();
                fs = new Friendship();

                fs.FriendID = id;
                fs.userID = currentuserId;
                fs.isApproved = true;

                db.Friendship.Add(fs);

                db.SaveChanges();
            }

            return RedirectToAction("MyFriends");
        }

        public ActionResult RemoveFriends(Guid id)
        {
            var f = db.Friendship.FirstOrDefault(i => i.Id == id);
            if (f != null)
            {
                db.Friendship.Remove(f);
                db.SaveChanges();
            }
            return RedirectToAction("MyFriends");
        }

        public ActionResult ListUsers()
        {
            var friends = userdb.Users.ToList();
            return View(friends);

        }

        public ActionResult ProfilEdit()
        {
            var currentuserId = User.Identity.GetUserId();
            var userDetail = userdb.Users.Find(currentuserId);
            return View(userDetail);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ProfilEdit(ApplicationUser model, HttpPostedFileBase uploadFile)
        {
            if (ModelState.IsValid)
            {
                var currentuserId = User.Identity.GetUserId();
                var userDetail = userdb.Users.Find(currentuserId);

                if (uploadFile != null && uploadFile.ContentLength > 0)
                {
                    var imageFilePath = "";
                    var fileName = "";

                    if (Path.GetExtension(uploadFile.FileName).ToLower() == ".jpg"
                        || Path.GetExtension(uploadFile.FileName).ToLower() == ".png"
                        || Path.GetExtension(uploadFile.FileName).ToLower() == ".gif"
                        || Path.GetExtension(uploadFile.FileName).ToLower() == ".jpeg")
                    {
                        fileName = Guid.NewGuid().ToString() + Path.GetExtension(uploadFile.FileName);
                        imageFilePath = Path.Combine(Server.MapPath("~/Uploads"), fileName);
                        uploadFile.SaveAs(imageFilePath);
                        ViewBag.UploadSuccess = true;
                        userDetail.ProfilePicture = "/Uploads/" + fileName;
                    }
                }

                userDetail.FirstName = model.FirstName;
                userDetail.LastName = model.LastName;
                userDetail.Gender = model.Gender;
                userDetail.Vision = model.Vision;
                userDetail.Mission = model.Mission;
                userDetail.PhoneNo = model.PhoneNo;

                userdb.Entry(userDetail).State = EntityState.Modified;
                userdb.SaveChanges();
                return RedirectToAction("Profil");
            }

            return View(model);
        }

        [Authorize]
        public ActionResult MailBox()
        {
            var currentUser = User.Identity.GetUserId();
            var viewData = db.PrivateMessages.Where(u => u.ReceiverId == currentUser);
            return View(viewData);
        }
        public ActionResult SendMail(string id)
        {
            var pm = new PrivateMessage();
            pm.SenderId = User.Identity.GetUserId();
            pm.ReceiverId = id;

            return View(pm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SendMail([Bind(Include = "SenderId,ReceiverId,Content,DateSent,isRead")] PrivateMessage privateMessage)
        {
            if (ModelState.IsValid)
            {
                privateMessage.isRead = false;
                privateMessage.DateSent = DateTime.Now;
                privateMessage.SenderId = User.Identity.GetUserId();

                db.PrivateMessages.Add(privateMessage);
                db.SaveChanges();
                return RedirectToAction("MailBox");
            }

            return View(privateMessage);
        }
        public ActionResult MailDetail(int id)
        {
            var pm = db.PrivateMessages.FirstOrDefault(i => i.Id == id);

            if (pm.isRead == false)
            {
                pm.isRead = true;

                db.Entry(pm).State = EntityState.Modified;
                userdb.SaveChanges();
            }

            return View(pm);
        }

        public ActionResult Files()
        {
            var curr = User.Identity.GetUserId();
            return View(db.Documents.Where(u => u.Owner == curr));
        }
    }
}
