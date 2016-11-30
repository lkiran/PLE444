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
            return PartialView(db.Courses.ToList());
        }

        [ChildActionOnly]
        public ActionResult Communities()
        {
            return PartialView(db.Communities.ToList());
        }

        public ActionResult LogedInUser()
        {
            var userID = User.Identity.GetUserId();
            return PartialView(UserDB.Users.FirstOrDefault(i => i.Id == userID));
        }
    }
}