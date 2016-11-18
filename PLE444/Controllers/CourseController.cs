using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PLE444.Context;
using PLE444.Models;

namespace PLE444.Controllers
{
    public class CourseController : Controller
    {
        private PleDbContext db = new PleDbContext();
        // GET: Course
        public ActionResult Index()
        {
            return View(db.Courses.ToList());
        }
        public ActionResult Assignments( Guid id)
        {
            PleDbContext db = new PleDbContext();
       
          var assignment = db.Assignments.Where(a => a.Course.ID == id).FirstOrDefault();

            return View(assignment);
        }
        public ActionResult Chapters()
        {
            return View();
        }
        // GET: Courses/Create
        public ActionResult CourseCreate()
        {
            return View();
        }

     
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CourseCreate([Bind(Include = "ID,Name,Description,CourseStart")] Course course)
        {
            if (ModelState.IsValid)
            {
              
                db.Courses.Add(course);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(course);
        }

        public ActionResult AssignmentCreate()
        {
            return View();
        }

  
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AssignmentCreate([Bind(Include = "Id,Title,Description,Deadline,DateAdded")] Assignment assignment)
        {
            if (ModelState.IsValid)
            {

                db.Assignments.Add(assignment);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(assignment);
        }
      
        public ActionResult Materials()
        {
            return View();
        }
        public ActionResult Grades()
        {
            return View();
        }
    }
}