using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PLE444.Controllers
{
    public class CommunityController : Controller
    {
        // GET: Community
        public ActionResult Index()
        {
            return View();
       }
        public ActionResult Events()
        {
            return View();
        }
    }
}