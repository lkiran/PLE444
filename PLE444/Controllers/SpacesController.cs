using PLE444.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace PLE444.Controllers
{
    public class SpacesController : Controller
    {
        private PleDbContext db = new PleDbContext();
        
        public ActionResult Index(int? id)
        {
            if (!id.HasValue)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            
            var viewData = new SpaceViewModel
            {
                SpaceInfo = db.Spaces.Find(id),
                Courses = db.Courses.Where(s => s.SpaceId == id && s.CanEveryoneJoin).ToList(),
                Communities = db.Communities.Where(s => s.SpaceId == id && !s.isHiden).ToList()
            };

            return View(viewData);
        }
    }
}