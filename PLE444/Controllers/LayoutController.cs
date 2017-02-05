using Microsoft.AspNet.Identity;

using PLE444.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;

namespace PLE444.Controllers
{
    public class LayoutController : Controller
    {
        private PleDbContext db = new PleDbContext();

        [ChildActionOnly]
        public ActionResult Courses()
        {
            var userId = User.Identity.GetUserId();

            if (userId.IsNullOrWhiteSpace())
                return PartialView(new List<Course>());

            var userCourses = db.UserCourses.Where(u => u.UserId == userId);
            var courses = db.Courses.Where(c => c.CreatorId == userId); 
            var data = (from p in userCourses select p.Course).Union(courses);

            ViewBag.CurrentUser = userId;
            return PartialView(data.ToList());
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
            var user = db.Users.FirstOrDefault(i => i.Id == userID);
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