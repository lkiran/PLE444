using Microsoft.AspNet.Identity;
using PLE444.Context;
using PLE444.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PLE444.Controllers
{
    public class LayoutController : Controller
    {
        private PleDbContext db = new PleDbContext();
        private ApplicationDbContext UserDB = new ApplicationDbContext();

        [ChildActionOnly]
        public ActionResult Courses()
        {
            var userID = User.Identity.GetUserId();
            var uc = db.UserCourses.Where(u => u.UserId == userID);
            var data = (from p in uc select p.Course).ToList();

            return PartialView(data);
        }

        [ChildActionOnly]
        public ActionResult Communities()
        {
            var userID = User.Identity.GetUserId();
            var uc = db.UserCommunities.Where(u => u.UserId == userID);
            var data = (from p in uc select p.Community).ToList();

            return PartialView(data);
        }

        public ActionResult LogedInUser()
        {
            var userID = User.Identity.GetUserId();
            var user = UserDB.Users.FirstOrDefault(i => i.Id == userID);
            return PartialView(user);
        }

        [ChildActionOnly]
        public ActionResult Spaces()
        {
            var s = db.Spaces.ToList();
            return PartialView(s);
        }
    }
}