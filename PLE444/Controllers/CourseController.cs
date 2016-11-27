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
using Microsoft.AspNet.Identity;

namespace PLE444.Controllers
{
    public class CourseController : Controller
    {
        private PleDbContext db = new PleDbContext();
        
        public ActionResult Index(Guid? id)
        {
            if (id == null)
                return View(db.Courses.ToList());
            return RedirectToAction("Chapters", new { id = id });
        }

        public ActionResult Assignments(Guid? id)
        {
            if (id == null)
                return RedirectToAction("Index");

            var c = db.Courses.Find(id);
            var assignment = db.Assignments.Include("Course").Where(a => a.Course.ID == id).ToList();
            ViewBag.CourseName = c.Name.ToUpper() + " - " + c.Description;
            ViewBag.CourseId = c.ID;
            return View(assignment);
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

        public ActionResult Chapters(Guid? id)
        {
            if (id == null)
                return RedirectToAction("Index");
            else
            {
                var c = db.Courses.Include("Chapters").Where(a => a.ID == id).FirstOrDefault();
                ViewBag.CourseName = c.Name.ToUpper() + " - " + c.Description;                     
                return View(c);                 
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
        
        public ActionResult Materials(Guid? id)
        {
            if (id == null)
                return RedirectToAction("Index");

            var c = db.Courses.Find(id);
            ViewBag.CourseName = c.Name.ToUpper() + " - " + c.Description;
            ViewBag.CourseId = c.ID;
            return View();
        }

        public ActionResult MeterialAdd(Guid? id)
        {
            if (id == null)
                RedirectToAction("Index");
            
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MeterialAdd(Material material, Guid chapterId)
        {
            if (ModelState.IsValid)
            {
                var c = db.Chapters.Find(chapterId);
            //    c.DateAdded = DateTime.Now;
            //    c.Title = chapter.Title;
            //    c.Description = chapter.Description;

            //    db.Chapters.Add(c);

            //    var co = db.Courses.Find(courseId);
            //    co.Chapters.Add(c);

            //    db.Entry(co).State = EntityState.Modified;

            //    db.SaveChanges();

                return RedirectToAction("ChapterList");
            }
            return View(material);
        }


        public ActionResult Grades(Guid? id)
        {
            if (id == null)
                return RedirectToAction("Index");

            var c = db.Courses.Find(id);
            ViewBag.CourseName = c.Name.ToUpper() + " - " + c.Description;
            ViewBag.CourseId = c.ID;
            return View();
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

        public ActionResult Discussion(Guid? id)
        {
            var c = db.Courses.Find(id);
            var m = db.Courses.Include("Discussion").Include("Discussion.Messages").FirstOrDefault(i => i.ID == id);

            ViewBag.Active = TempData["Active"];
            ViewBag.CourseName = c.Name.ToUpper() + " - " + c.Description;
            ViewBag.CurrentUserId = User.Identity.GetUserId();

            return View(m);
        }

        public ActionResult AddTitle(string id)
        {
            ViewBag.CourseId = id;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddTitle(Discussion discussion, Guid courseId)
        {
            if (ModelState.IsValid)
            {
                var d = new Discussion();
                d.DateCreated = DateTime.Now;
                d.CreatorId = User.Identity.GetUserId();
                d.Topic = discussion.Topic;

                db.Discussions.Add(d);

                var c = db.Courses.Find(courseId);
                c.Discussion.Add(d);

                db.Entry(c).State = EntityState.Modified;

                db.SaveChanges();

                return RedirectToAction("Discussion", new { id = courseId });
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SendMessage(Message message, Guid courseId, Guid discussionId)
        {
            if (ModelState.IsValid)
            {
                var m = new Message();
                m.Content = message.Content;
                m.DateSent = DateTime.Now;
                m.SenderId = User.Identity.GetUserId();

                db.Messages.Add(m);

                var d = db.Discussions.Find(discussionId);
                d.Messages.Add(m);

                db.Entry(d).State = EntityState.Modified;

                db.SaveChanges();

                TempData["Active"] = discussionId;
                return RedirectToAction("Discussion", new { id = courseId });
            }
            return View();
        }


    }
}