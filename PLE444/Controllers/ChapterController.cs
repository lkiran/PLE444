using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using PLE444.Models;
using PLE444.ViewModels;

namespace PLE444.Controllers
{
    public class ChapterController : Controller
    {
        private PleDbContext db = new PleDbContext();

        public ActionResult Index(Guid? id)
        {
            if (id == null)
                return RedirectToAction("Index", "Home");
            else
            {
                var course = db.Courses.SingleOrDefault(i => i.ID == id);
                var chapters = db.Chapters.Include("Materials").Where(i => i.CourseId == id).ToList();

                var model = new Chapters
                {
                    canEdit = User.Identity.GetUserId() == course.CreatorId ? true : false,
                    CourseInfo = course,
                    ChapterList = chapters
                };

                return View(model);
            }
        }

        [Authorize]
        public ActionResult Create(Guid id)
        {
            if (!isCourseCreator(id))
                return RedirectToAction("Index", "Home");

            var model = new Chapter
            {
                CourseId = id
            };
            return View(model);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Chapter chapter)
        {
            if (!isCourseCreator(chapter.CourseId))
                return RedirectToAction("Index", "Home");

            else if (ModelState.IsValid)
            {
                var c = new Chapter
                {
                    DateAdded = DateTime.Now,
                    Title = chapter.Title,
                    Description = chapter.Description
                };

                db.Chapters.Add(c);

                var co = db.Courses.Find(chapter.CourseId);
                co.Chapters.Add(c);

                db.Entry(co).State = EntityState.Modified;

                db.SaveChanges();

                return RedirectToAction("Index", new { id = chapter.CourseId });
            }

            return View(chapter);
        }

        [Authorize]
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var model = db.Chapters.Find(id);
            if (model == null)
                HttpNotFound();

            else if (!isCourseCreator(model.CourseId))
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);

            return View(model);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Chapter model)
        {
            if (ModelState.IsValid)
            {
                if (!isCourseCreator(model.CourseId))
                    return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);

                var chapterDb = db.Chapters.Find(model.Id);
                chapterDb.Description = model.Description;
                chapterDb.Title = model.Title;
                
                db.Entry(chapterDb).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index", new { id = model.CourseId });
            }

            return View(model);
        }

        public bool isCourseCreator(Guid? courseId)
        {
            if (courseId == null)
                return false;

            var c = db.Courses.Find(courseId);

            if (c.CreatorId != User.Identity.GetUserId())
                return false;
            return true;
        }

    }
}
