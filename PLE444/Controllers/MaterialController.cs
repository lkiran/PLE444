using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PLE444.Models;
using Microsoft.AspNet.Identity;
using System.IO;
using Microsoft.Ajax.Utilities;
using PLE444.ViewModels;

namespace PLE444.Controllers
{
    public class MaterialController : Controller
    {
        private PleDbContext db = new PleDbContext();

        [Authorize]
        public ActionResult Index(Guid? id)
        {
            if (id == null)
                return RedirectToAction("Index", "Home");

            var chapters = db.Chapters.Where(c => c.Course.ID == id).Include("Materials").Include("Materials.Documents");

            var model = new CourseMaterials
            {
                CourseId = (Guid)id,
                CanEdit = isCourseCreator(id),
                ChapterList = chapters
            };
            return View(model);
        }

        [Authorize]
        public ActionResult Add(Guid? chapterId)
        {
            if (chapterId == null)
                return RedirectToAction("Index");

            var courseId = db.Chapters.Find(chapterId).CourseId;
            if (!isCourseCreator(courseId))
                return RedirectToAction("Index");

            var model = new MaterialForm
            {
                ChapterId = (Guid) chapterId,
            };

            return View(model);
        }


        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Add(MaterialForm model)
        {
            var chapter = db.Chapters.Find(model.ChapterId);

            if (!isCourseCreator(chapter.CourseId))
                return RedirectToAction("Index");

            else if (ModelState.IsValid)
            {
                foreach (var uploadedFile in model.UploadedFiles)  //iterate in each file
                {
                    if (uploadedFile != null && uploadedFile.ContentLength > 0) //check length of bytes are greater then zero or not
                    {
                        var fileName = "";
                        fileName = Guid.NewGuid().ToString() + Path.GetExtension(uploadedFile.FileName);
                        var imageFilePath = Path.Combine(Server.MapPath("~/Uploads"), fileName);
                        uploadedFile.SaveAs(imageFilePath);
                        ViewBag.UploadSuccess = true;

                        //Add to DB
                        var d = new Document
                        {
                            FilePath = "/ Uploads / " + fileName,
                            Owner = User.Identity.GetUserId(),
                            DateUpload = DateTime.Now,
                            Description = uploadedFile.FileName
                        };

                        var doc = db.Documents.Add(d);

                        model.Documents.Add(doc);
                    }
                }
           
                var material = new Material
                {
                    Id = Guid.NewGuid(),
                    DateAdded = DateTime.Now,
                    Description = model.Description,
                    Documents = model.Documents,
                    Title = model.Title
                };

                chapter.Materials.Add(material);
                db.Entry(chapter).State = EntityState.Modified;

                db.SaveChanges();

                return RedirectToAction("Index", "Chapter", new {id = chapter.CourseId});
            }
            return View(model);
        }

        public ActionResult Update(Guid? chapterId, Guid? Id)
        {
            if (Id == null || chapterId == null)
                return RedirectToAction("Index");

            var courseId = db.Chapters.Find(chapterId).CourseId;
            if (!isCourseCreator(courseId))
                return RedirectToAction("Index");

            var material = db.Materials.SingleOrDefault(i => i.Id == Id);
            if (material == null)
                return RedirectToAction("Index");

            var model = new MaterialForm
            {
                Id = material.Id,
                ChapterId = (Guid) chapterId,
                Description = material.Description,
                Documents = material.Documents.ToList(),
                Title = material.Title
            };

            return View(model);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Update(MaterialForm model)
        {
            var chapter = db.Chapters.Find(model.ChapterId);

            if (!isCourseCreator(chapter.CourseId))
                return RedirectToAction("Index");

            else if (ModelState.IsValid)
            {
                foreach (var uploadedFile in model.UploadedFiles)  //iterate in each file
                {
                    if (uploadedFile != null && uploadedFile.ContentLength > 0) //check length of bytes are greater then zero or not
                    {
                        var fileName = "";
                        fileName = Guid.NewGuid().ToString() + Path.GetExtension(uploadedFile.FileName);
                        var imageFilePath = Path.Combine(Server.MapPath("~/Uploads"), fileName);
                        uploadedFile.SaveAs(imageFilePath);
                        ViewBag.UploadSuccess = true;

                        //Add to DB
                        var d = new Document
                        {
                            FilePath = "/ Uploads / " + fileName,
                            Owner = User.Identity.GetUserId(),
                            DateUpload = DateTime.Now,
                            Description = uploadedFile.FileName
                        };

                        var doc = db.Documents.Add(d);

                        model.Documents.Add(doc);
                    }
                }

                var material = db.Materials.Find(model.Id);
                material.Description = model.Description;
                material.Documents = model.Documents;
                material.Title = model.Title;

                db.Entry(material).State = EntityState.Modified;

                db.SaveChanges();

                return RedirectToAction("Index", "Chapter", new { id = chapter.CourseId });
            }
            return View(model);
        }

        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Material material = db.Materials.Find(id);
            if (material == null)
            {
                return HttpNotFound();
            }
            return View(material);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Description,DateAdded")] Material material)
        {
            if (ModelState.IsValid)
            {
                db.Entry(material).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(material);
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
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

        private bool isMember(Guid? courseId)
        {
            if (courseId == null)
                return false;

            var userId = User.Identity.GetUserId();
            var user = db.UserCourses.Where(c => c.Course.ID == courseId).FirstOrDefault(u => u.UserId == userId);

            if (user == null)
                return false;
            return true;
        }
    }
}