using PLE444.Context;
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
        private PleDbContext db = new PleDbContext();
        public ActionResult List() {
            var com = db.Communities.ToList();
            return View(com);
        }
        public ActionResult Index()
        {
            return View();
       }
       
        public ActionResult Events()
        {
            return View();
        }
        public ActionResult Discussion()
        {
            return View();
        }
        public ActionResult Archive()
        {
            return View();
        }

    }
}