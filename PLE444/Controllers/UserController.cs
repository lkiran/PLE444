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
    
        [Authorize]
      
        public ActionResult ListUsers()
        {
            var friends = userdb.Users.ToList();
            return View(friends);
        }
    }
}
