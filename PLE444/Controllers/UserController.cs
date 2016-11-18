using Microsoft.AspNet.Identity;
using PLE444.Context;
using PLE444.Models;
using System;
using System.Collections.Generic;
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
      
        public ActionResult ListUsers()
        {
            var friends = userdb.Users.ToList();
            return View(friends);
        }
        public ActionResult AddFriend(String id)
        {
           var currentuserId = User.Identity.GetUserId();
           var friends = userdb.Users.ToList();
           var fs = new Friendship();
            fs.FriendID = id;
            fs.userID = currentuserId;
            fs.isApproved = true;
           db.Friendship.Add(fs);
           db.SaveChanges();
           
            return RedirectToAction("ListUsers");
        }
        public ActionResult MyFriends()
        {

            return View();
        }
    }
}
