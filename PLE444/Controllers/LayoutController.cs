using PLE444.Context;
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
    }
}