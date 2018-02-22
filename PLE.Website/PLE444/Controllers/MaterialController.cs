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

            if (!isMember(id) && !isCourseCreator(id))
                return RedirectToAction("Index", "Course", new { id = id });

            var chapters =
                db.Chapters.Where(c => c.Course.Id == id).Select(e => new
                {
                    e, Materials = e.Materials.Where(m => m.IsActive)
                }).AsEnumerable().Select(e => e.e).ToList();

			var course = db.Courses.FirstOrDefault(i => i.Id == id);

			var model = new CourseMaterials
			{
				CourseId = (Guid)id,
				CanEdit = isCourseCreator(id),
				ChapterList = chapters,
				IsItActive = course.IsCourseActive
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
                var currentUserId = User.GetPrincipal()?.User.Id;
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
                            FilePath = "~/Uploads/" + fileName,
                            OwnerId = currentUserId,
                            DateUpload = DateTime.Now,
                            Description = uploadedFile.FileName
                        };

                        var doc = db.Documents.Add(d);
                        model.OwnerId = currentUserId;
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

        [Authorize]
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
                        var imageFilePath = Path.Combine(Server.MapPath("~/Uploads/"), fileName);
                        uploadedFile.SaveAs(imageFilePath);
                        ViewBag.UploadSuccess = true;

                        //Add to DB
                        var d = new Document
                        {
                            FilePath = "~/Uploads/" + fileName,
                            OwnerId = User.GetPrincipal()?.User.Id,
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

        public ActionResult Detail(Guid? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            
            var material = db.Materials.Include(d => d.Documents).FirstOrDefault(m => m.Id == id);

            if (material == null)
                return HttpNotFound();

            return PartialView(material);
        }


		[HttpPost]
		[Authorize]
		public JsonResult RemoveFromChapter(Guid? chapterId, Guid? materialId)
		{
			if (chapterId == null || materialId == null)
				return Json(new { Success = false, Message = "BadRequest" }, JsonRequestBehavior.AllowGet);

			var chapter = db.Chapters.Find(chapterId);
			if (chapter == null)
				return Json(new { Success = false, Message = "HttpNotFound" }, JsonRequestBehavior.AllowGet);

			else if (!isCourseCreator(chapter.CourseId))
				return Json(new { Success = false, Message = "Unauthorized" }, JsonRequestBehavior.AllowGet);

			var material = chapter.Materials.FirstOrDefault(m => m.Id == materialId);
			if (material == null)
				return Json(new { Success = false, Message = "HttpNotFound" }, JsonRequestBehavior.AllowGet);

			material.IsActive = false;

			chapter.Materials.Remove(material);

			db.Entry(material).State = EntityState.Modified;
			db.Entry(chapter).State = EntityState.Modified;
			db.SaveChanges();

			return Json(new { Success = true, Message = "OK" }, JsonRequestBehavior.AllowGet);
		}

		//[HttpPost]
		//[Authorize]
		//public JsonResult Delete(Chapter chapter, Guid? materialId)
		//{
		//	if (materialId == null)
		//		return Json(new { Success = false, Message = "BadRequest" }, JsonRequestBehavior.AllowGet);

		//	var material = db.Materials.Find(materialId);
		//	if (material == null)
		//		return Json(new { Success = false, Message = "HttpNotFound" }, JsonRequestBehavior.AllowGet);

		//	else if (!isCourseCreator(chapter.CourseId))
		//		return Json(new { Success = false, Message = "Unauthorized" }, JsonRequestBehavior.AllowGet);

		//	material.IsActive = false;

		//	db.Entry(material).State = EntityState.Modified;
		//	db.SaveChanges();

		//	return Json(new { Success = true, Message = "OK" }, JsonRequestBehavior.AllowGet);
		//}

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

            if (c.CreatorId != User.GetPrincipal()?.User.Id)
                return false;
            return true;
        }

        private bool isMember(Guid? courseId)
        {
            if (courseId == null)
                return false;

            var userId = User.GetPrincipal()?.User.Id;
            var user = db.UserCourses.Where(c => c.Course.Id == courseId).FirstOrDefault(u => u.UserId == userId);

            if (user == null)
                return false;
            else
                return user.IsActive && user.DateJoin != null;
        }

        private bool isMember(Course course)
        {
            return isMember(course.Id);
        }

        private bool isWaiting(Guid? courseId)
        {
            var userId = User.GetPrincipal()?.User.Id;
            var user = db.UserCourses.Where(c => c.Course.Id == courseId && c.IsActive).FirstOrDefault(u => u.UserId == userId);
            if (user == null)
                return false;
            return user.DateJoin == null;
        }

        private bool isOwner(string id)
        {
            var userId = User.GetPrincipal()?.User.Id;
            if (userId == id)
                return true;
            return false;
        }
    }
}