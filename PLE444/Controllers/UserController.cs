using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
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
using PLE444.ViewModels;

namespace PLE444.Controllers
{
    public class UserController : Controller
    {
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

            var userDetail = db.Users.Find(id);

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
                var friends = db.Users.ToList();
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
            var friends = db.Users.ToList();
            return View(friends);

        }

        public ActionResult ProfilEdit()
        {
            var currentuserId = User.Identity.GetUserId();
            var userDetail = db.Users.Find(currentuserId);
            return View(userDetail);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ProfilEdit(ApplicationUser model, string photoBase64)
        {
            if (ModelState.IsValid)
            {
                var currentuserId = User.Identity.GetUserId();
                var userDetail = db.Users.Find(currentuserId);

                if (!photoBase64.IsNullOrWhiteSpace())
                {
                    IList<string> data = photoBase64.Split(',').ToList<string>();
                    byte[] imageInBytes = Convert.FromBase64String(data[1]);
                    var newPhotoName = Guid.NewGuid() + "." + data[0].Split('/')[1].Split(';')[0];

                    using (var imageFile = new FileStream(Path.Combine(Server.MapPath("~/Uploads"), newPhotoName), FileMode.Create))
                    {
                        imageFile.Write(imageInBytes, 0, imageInBytes.Length);
                        imageFile.Flush();
                    }

                    //Delete previous profile picture from /Uploads folder
                    var prevPath = Server.MapPath(userDetail.ProfilePicture);
                    var prevPhoto = new FileInfo(prevPath);
                    prevPhoto.Delete();

                    model.ProfilePicture = "~/Uploads/" + newPhotoName;
                }

                userDetail.FirstName = model.FirstName;
                userDetail.LastName = model.LastName;
                userDetail.Gender = model.Gender;
                userDetail.Vision = model.Vision;
                userDetail.Mission = model.Mission;
                userDetail.PhoneNo = model.PhoneNo;
                userDetail.ProfilePicture = model.ProfilePicture;

                db.Entry(userDetail).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Profil");
            }

            return View(model);
        }

        [Authorize]
        public ActionResult MailBox()
        {
            var currentUser = User.Identity.GetUserId();

            var model = new MailBox
            {
                Inbox = db.PrivateMessages
                    .Where(u => u.ReceiverId == currentUser)
                    .Include("Sender").Include("Receiver").ToList(),

                Sent = db.PrivateMessages
                    .Where(u => u.SenderId == currentUser)
                    .Include("Sender").Include("Receiver").ToList()
            };

            return View(model);
        }
        public ActionResult SendMail(string id)
        {
            var model = new PrivateMessage
            {
                Sender = db.Users.Find(User.Identity.GetUserId()),
                Receiver = db.Users.Find(id)
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SendMail(PrivateMessage privateMessage)
        {
            var currentUserId = User.Identity.GetUserId();
        
            db.PrivateMessages.Add(new PrivateMessage
            {
                Content = privateMessage.Content,
                DateSent = DateTime.Now,
                IsRead = false,
                ReceiverId = privateMessage.Receiver.Id,
                SenderId = currentUserId
            });
            db.SaveChanges();

            return RedirectToAction("MailBox");
        }
        public ActionResult MailDetail(int id)
        {
            var pm = db.PrivateMessages.Include("Sender").Include("Receiver").FirstOrDefault(i => i.Id == id);

            pm.IsRead = true;

            db.Entry(pm).State = EntityState.Modified;
            db.SaveChanges();

            return View(pm);
        }

        public ActionResult Files()
        {
            var curr = User.Identity.GetUserId();
            return View(db.Documents.Where(u => u.OwnerId == curr));
        }
    }
}
