using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Ionic.Zip;
using Microsoft.AspNet.Identity;
using PLE444.Models;
using PLE444.ViewModels;

namespace PLE444.Controllers
{
    [Authorize]
    public class AssignmentController : Controller
    {
        private PleDbContext db = new PleDbContext();

        public ActionResult Index(Guid? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var course = db.Courses.Find(id);
            if (course == null)
                return  HttpNotFound();

            if (!isMember(id) && !isCourseCreator(course))
                return RedirectToAction("Index", "Course", new { id = id });

            var assignments = db.Assignments.Include("Uploads").Include("Uploads.Owner").Where(a => a.Course.Id == id && a.IsActive).ToList();
            var model = new CourseAssignments
            {
                CourseInfo = course,
                AssignmentList = assignments,
                CanEdit = isCourseCreator(course),
                CanUpload = isMember(course.Id),
                CurrentUserId = User.Identity.GetUserId()
            };

            return View(model);
        }

        public ActionResult Create(Guid id)
        {
            if (!isCourseCreator(id))
                return RedirectToAction("Index", "Home");

            var model = new AssignmentForm
            {
                CourseId = id,
                Deadline = DateTime.Now
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AssignmentForm model)
        {
            var course = db.Courses.Find(model.CourseId);

            if (!isCourseCreator(course))
                return RedirectToAction("Index", "Home");

            else if (ModelState.IsValid)
            {
                var assignment = new Assignment
                {
                    DateAdded = DateTime.Now,
                    Title = model.Title,
                    Description = model.Description,
                    Deadline = model.Deadline
                };
                db.Assignments.Add(assignment);

                course.Assignments.Add(assignment);
                db.Entry(course).State = EntityState.Modified;

                db.SaveChanges();

                return RedirectToAction("Index", "Assignment", new {id = model.CourseId});
            }

            return View(model);
        }

        public ActionResult Edit(Guid? id, Guid? courseId)
        {
            if (!id.HasValue || !courseId.HasValue)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            if (!isCourseCreator(courseId))
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);

            var assignment = db.Assignments.Include("Course").FirstOrDefault(i => i.Id == id);
            if (assignment == null)
                return HttpNotFound();

            var model = new AssignmentForm
            {
                Id = assignment.Id,
                CourseId = assignment.CourseId,
                Deadline = assignment.Deadline,
                Description = assignment.Description,
                Title = assignment.Title
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(AssignmentForm model)
        {
            if (ModelState.IsValid)
            {
                var assignment = db.Assignments.Include("Course").SingleOrDefault(i => i.Id == model.Id);
                if (assignment == null)
                    return HttpNotFound();

                if (!isCourseCreator(assignment.Course))
                    return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);

                assignment.Deadline = model.Deadline;
                assignment.Description = model.Description;
                assignment.Title = model.Title;

                db.Entry(assignment).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index", "Assignment", new {id = assignment.CourseId});
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
                return Json(new { Success = false, Message = "BadRequest" }, JsonRequestBehavior.AllowGet);

            var assignment = db.Assignments.Find(id);
            if (assignment == null)
                return Json(new { Success = false, Message = "HttpNotFound" }, JsonRequestBehavior.AllowGet);

            else if (!isCourseCreator(assignment.CourseId))
                return Json(new { Success = false, Message = "Unauthorized" }, JsonRequestBehavior.AllowGet);

            assignment.IsActive = false;

            db.Entry(assignment).State = EntityState.Modified;
            db.SaveChanges();

            return Json(new { Success = true, Message = "OK" }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Upload(Guid assignmentId, HttpPostedFileBase uploadFile)
        {
            var a = db.Assignments.Include("Course").FirstOrDefault(i => i.Id == assignmentId);

            if (a == null || !isMember(a.Course.Id))
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);

            if (ModelState.IsValid)
            {
                var currentuserId = User.Identity.GetUserId();

                if (uploadFile != null && uploadFile.ContentLength > 0)
                {
                    var filePath = "";
                    var fileName = "";

                    fileName = Guid.NewGuid().ToString() + Path.GetExtension(uploadFile.FileName);
                    filePath = Path.Combine(Server.MapPath("~/Uploads"), fileName);
                    uploadFile.SaveAs(filePath);
                    ViewBag.UploadSuccess = true;

                    var uploaded = a.Uploads.FirstOrDefault(u => u.OwnerId == currentuserId);
                    if (uploaded == null)
                    {
                        var d = new Document
                        {
                            DateUpload = DateTime.Now,
                            Description = uploadFile.FileName,
                            OwnerId = currentuserId,
                            FilePath = "~/Uploads/" + fileName
                        };

                        db.Documents.Add(d);
                        a.Uploads.Add(d);
                    }
                    else
                    {
                        uploaded.DateUpload = DateTime.Now;
                        uploaded.Description = uploadFile.FileName;
                        uploaded.FilePath = "~/Uploads/" + filePath;
                        uploaded.OwnerId = currentuserId;

                        db.Entry(uploaded).State = EntityState.Modified;
                    }

                    db.Entry(a).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }

            return RedirectToAction("Index", "Assignment", new {id = a.Course.Id});
        }

        public ActionResult DownloadAssignment(Guid asssignmentId)
        {
            var assignment = db.Assignments.Include("Uploads").Include("Uploads.Owner").FirstOrDefault(a => a.Id == asssignmentId);
            
            if (assignment == null)
                return HttpNotFound();

            if(!isCourseCreator(assignment.CourseId))
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);

            var documents = assignment.Uploads.ToList();

            var memoryStream = new MemoryStream();

            using (var zip = new ZipFile())
            {
                foreach (var document in documents)
                {
                    var path = Server.MapPath(document.FilePath);

                    if (System.IO.File.Exists(path))
                    {
                        var zipFileName = document.Owner.FullName() + "_" + document.Description;
                        zip.AddFile(path, "").FileName = zipFileName;
                    }
                }
                zip.Save(memoryStream);
            }
            memoryStream.Seek(0, 0);
            return File(memoryStream, "application/octet-stream", DateTime.Now + ".zip");
        }

        private bool isCourseCreator(Guid? courseId)
        {
            if (courseId == null)
                return false;

            var course = db.Courses.Find(courseId);
            return isCourseCreator(course);
        }

        private bool isCourseCreator(Course course)
        {
            if (course == null)
                return false;

            else if (course.CreatorId != User.Identity.GetUserId())
                return false;
            return true;
        }

        private bool isMember(Guid? courseId)
        {
            if (courseId == null)
                return false;

            var userId = User.Identity.GetUserId();
            var user = db.UserCourses.Where(c => c.Course.Id == courseId).FirstOrDefault(u => u.UserId == userId && u.IsActive);

            if (user == null)
                return false;
            return true;
        }
    }
}