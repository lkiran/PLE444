using Microsoft.AspNet.Identity;
using PLE444.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PLE444.Controllers
{
    public class UserController : Controller
    {
        private PleDbContext db = new PleDbContext();
        [Authorize]
        public ActionResult Index()
        {
            var currentuserId = User.Identity.GetUserId();
            var userDetail = db.UserDetails.Find(currentuserId);

            return View(userDetail);
        }
    }
}
