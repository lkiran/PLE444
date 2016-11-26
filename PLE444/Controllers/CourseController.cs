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
        
        public ActionResult Index()
        {
            return View(db.Courses.ToList());
        }
        public ActionResult ChapterList()
        {
            return View(db.Chapters.ToList());
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
       
        public ActionResult CourseCreate()
        {
            return View();
        }
     
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

        [HttpPost, ActionName("CourseDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Course course = db.Courses.Find(id);
            db.Courses.Remove(course);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult AssignmentCreate(Guid id)
        {
           
            ViewBag.CourseID = db.Courses.Find(id);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AssignmentCreate([Bind(Include = "Id,Title,Description,Deadline,DateAdded")] Assignment assignment)
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

        public ActionResult Chapters(Guid id)
        {
            var c = db.Courses.Find(id);
            var chapter = db.Chapters.Where(a => a.CourseId == id).ToList();
            ViewBag.CourseName = c.Name.ToUpper() + " - " + c.Description;
            if (id == null)
            {

                return RedirectToAction("Index");
            }
            else
            {
                return View(chapter);
            }
           
        }

        public ActionResult ChapterCreate(string id)
        {
            ViewBag.CourseId = id;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChapterCreate( Chapter chapter, Guid courseId)
        {
            if (ModelState.IsValid)
            {
                var c = new Chapter();
                c.DateAdded = DateTime.Now;
                c.Title = chapter.Title;
                c.Description = chapter.Description;

                db.Chapters.Add(c);

                var co = db.Courses.Find(courseId);
                co.Chapters.Add(c);

                db.Entry(co).State = EntityState.Modified;

                db.SaveChanges();

                return RedirectToAction("ChapterList");
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