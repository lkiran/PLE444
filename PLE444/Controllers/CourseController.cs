using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
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
            var c = db.Courses.Find(id);
          var assignment = db.Assignments.Where(a => a.Course.ID == id).FirstOrDefault();
            ViewBag.CourseName = c.Name.ToUpper() + " - " + c.Description;
            return View(assignment);
        }
        public ActionResult CourseDetails(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.Courses.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }

        // GET: Courses/Edit/5
        public ActionResult CourseEdit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.Courses.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }

        // POST: Courses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CourseEdit([Bind(Include = "ID,Name,Description,CourseStart")] Course course)
        {
            if (ModelState.IsValid)
            {
                db.Entry(course).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(course);
        }
        public ActionResult Chapters( Guid id)
        {
            var c = db.Courses.Find(id);
            var chapter= db.Chapters.Where(a => a.Course.ID == id).FirstOrDefault();
            ViewBag.CourseName = c.Name.ToUpper() + " - " + c.Description;
            return View(chapter);
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
        public ActionResult CourseDelete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.Courses.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }

        // POST: Courses/Delete/5
        [HttpPost, ActionName("CourseDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Course course = db.Courses.Find(id);
            db.Courses.Remove(course);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
      //  public ActionResult AssignmentCreate()
              public ActionResult AssignmentCreate(Guid id)
        {
           
            ViewBag.CourseID = db.Courses.Find(id);
            return View();
        }

  
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AssignmentCreate([Bind(Include = "Id,Title,Description,Deadline,DateAdded")] Assignment assignment)
            // public ActionResult AssignmentCreate([Bind(Include = "Id,Title,Description,Deadline,DateAdded,Course")] Assignment assignment)
        {
            if (ModelState.IsValid)
            {
                assignment.Course = ViewBag.CourseID;
                assignment.DateAdded = DateTime.Now;
                db.Assignments.Add(assignment);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(assignment);
        }
        public ActionResult ChapterCreate()
        {
            return View();
        }

        // POST: Chapters/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChapterCreate([Bind(Include = "Id,Title,Description,DateAdded")] Chapter chapter)
        {
            if (ModelState.IsValid)
            {
                //chapter.Id = Guid.NewGuid();
                db.Chapters.Add(chapter);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(chapter);
        }

        public ActionResult Materials()
        {
            return View();
        }
        public ActionResult Grades()
        {
            return View();
        }
        
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

    }
}