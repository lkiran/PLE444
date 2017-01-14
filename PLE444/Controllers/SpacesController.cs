using PLE444.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PLE444.Controllers
{
    public class SpacesController : Controller
    {
        private PleDbContext db = new PleDbContext();
        
        public ActionResult Index(int? id)
        {
            if(id!=null)
            {
                var viewData = new SpaceViewModel();
                viewData.SpaceInfo = db.Spaces.Find(id);
                viewData.Courses = db.Courses.Where(s => s.SpaceId == id).ToList();
                viewData.Communities = db.Communities.Where(s => s.SpaceId == id).ToList();

                return View(viewData);
            }
            return RedirectToAction("Profil", "User");
        }
    }
}