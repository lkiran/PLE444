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
using System.IO;

namespace PLE444.Controllers
{
    public class CourseController : Controller
    {
        private PleDbContext db = new PleDbContext();
        private ApplicationDbContext userDb = new ApplicationDbContext();

        public ActionResult Index(Guid? id)
        {
            if (id == null)
            {
                 var c = db.Courses.ToList();
                var ui = User.Identity.GetUserId();
                var uc = db.UserCourses.Where(i => i.UserId == ui);
                var joinList = new List<bool>();
                foreach (var item in c)
                {
                    var r = uc.FirstOrDefault(i => i.Course.ID == item.ID);
                    if (r == null)
                        joinList.Add(false);
                    else
                        joinList.Add(true);
                }
                ViewBag.JoinList = joinList;
                return View(c);
            }
            return RedirectToAction("Chapters", new { id = id });
        }

        [Authorize]
        public ActionResult Assignments(Guid? id)
        {
            if (id == null)
                return RedirectToAction("Index");

            var c = db.Courses.FirstOrDefault(i => i.ID == id);
            var assignment = db.Assignments.Include("Course").Include("Uploads").Where(a => a.Course.ID == id).ToList();

            ViewBag.CourseName = c.Name.ToUpper() + " - " + c.Description;
            ViewBag.CourseId = c.ID;
            ViewBag.CurrentUser = User.Identity.GetUserId();
            return View(assignment);
        }

        [Authorize]
        public ActionResult AssignmentCreate(string id)
        {
            ViewBag.CourseId = id;
            return View();
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult AssignmentCreate(Assignment assignment, Guid courseId)
        {
            if(!isCourseCreator(courseId))
                return RedirectToAction("Index");

            if (ModelState.IsValid)
            {
                var c = new Assignment();
                c.DateAdded = DateTime.Now;
                c.Title = assignment.Title;
                c.Description = assignment.Description;
                c.Deadline = assignment.Deadline;
                db.Assignments.Add(c);

                var co = db.Courses.Find(courseId);
                co.Assignments.Add(c);

                db.Entry(co).State = EntityState.Modified;

                db.SaveChanges();

                return RedirectToAction("Assignments", new { id = courseId });
            }
            ViewBag.CourseId = courseId;
            return View(assignment);
        }
        
        public ActionResult AssignmentEdit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Assignment assignment = db.Assignments.Include("Course").FirstOrDefault(i => i.Id == id);
            if (assignment == null)
            {
                return HttpNotFound();
            }
           
            return View(assignment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AssignmentEdit(Assignment assignment, Guid courseId)
        {
            if (ModelState.IsValid)
            {       
                var co = db.Courses.Find(courseId);
                assignment.Course = co;
                assignment.DateAdded = DateTime.Now;

                db.Entry(assignment).State = EntityState.Modified;

                db.SaveChanges();
                return RedirectToAction("Assignments");
            }
            
            return View(assignment);
        }

        public ActionResult AssignmentDelete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Assignment assignment = db.Assignments.Find(id);
            if (assignment == null)
            {
                return HttpNotFound();
            }
            return View(assignment);
        }

        [HttpPost, ActionName("AssignmentDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed2(Guid id)
        {
            Assignment assignment = db.Assignments.Find(id);
            db.Assignments.Remove(assignment);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AssingmentUpload(Guid assignmentId, HttpPostedFileBase uploadFile)
        {
            var a = db.Assignments.Include("Course").FirstOrDefault(i => i.Id == assignmentId);

            if (!isMember(a.Course.ID)) 
                return RedirectToAction("Index");

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

                    var uploaded = a.Uploads.FirstOrDefault(u => u.Owner == currentuserId);
                    if(uploaded ==null)
                    {
                        var d = new Document();
                        d.DateUpload = DateTime.Now;
                        d.Description = uploadFile.FileName;
                        d.Owner = currentuserId;
                        d.FilePath = filePath;

                        db.Documents.Add(d);
                        a.Uploads.Add(d);
                    }
                    else
                    {
                        uploaded.DateUpload = DateTime.Now; ;
                        uploaded.Description = uploadFile.FileName;
                        uploaded.FilePath = filePath;
                        uploaded.Owner = currentuserId;

                        db.Entry(uploaded).State = EntityState.Modified;
                    }
                    
                    db.Entry(a).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
            
            return RedirectToAction("Assignments", new { id = a.Course.ID });
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

        [Authorize]
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
            ViewBag.Sapces = db.Spaces.ToList();
            return View(course);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult CourseEdit([Bind(Include = "ID,Name,Description,CourseStart,SpaceId")] Course course)
        {
            if (ModelState.IsValid)
            {
                db.Entry(course).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Sapces = db.Spaces.ToList();
            return View(course);
        }

        [Authorize]
        public ActionResult CourseCreate()
        {
            ViewBag.Sapces = db.Spaces.ToList();
            return View();
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult CourseCreate([Bind(Include = "ID,Name,Description,CourseStart,SpaceId")] Course course)
        {
            if (ModelState.IsValid)
            {
                course.CreatorId = User.Identity.GetUserId();
                db.Courses.Add(course);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Sapces = db.Spaces.ToList();
            return View(course);
        }

        [Authorize]
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
                var c = db.Courses.Include("Chapters").Include("Chapters.Materials").Where(a => a.ID == id).FirstOrDefault();
                ViewBag.CourseName = c.Name.ToUpper() + " - " + c.Description;
                ViewBag.CurrentUser = User.Identity.GetUserId();
                return View(c);
            }
        }
        public ActionResult ChapterDetails(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Chapter chapter = db.Chapters.Find(id);
            if (chapter == null)
            {
                return HttpNotFound();
            }
            return View(chapter);
        }

        public ActionResult ChapterCreate(string id)
        {
            if (!isCourseCreator(Guid.Parse(id)))
                return RedirectToAction("Index");

            ViewBag.CourseId = id;
            return View();
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult ChapterCreate(Chapter chapter, Guid courseId)
        {
            if (!isCourseCreator(courseId))
                return RedirectToAction("Index");

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

                return RedirectToAction("Index", new { id = courseId });
            }



            return View(chapter);
        }

        [Authorize]
        public ActionResult Materials(Guid? id)
        {
            ViewBag.isCrator = false;
            var course = db.Courses.Find(id);
            if (id == null)
                return RedirectToAction("Index");

            var c = db.Chapters.Include("Materials").Include("Materials.Documents").Where(i => i.CourseId == id).ToList();

            var materials = new List<Material>();

            foreach (var item in c)
                materials.AddRange(item.Materials);

            if (User.Identity.GetUserId() == course.CreatorId)
                ViewBag.isCrator = true;

            ViewBag.CourseName = course.Name.ToUpper() + " - " + course.Description;
            ViewBag.CourseId = course.ID;
            return View(materials);
        }

        [Authorize]
        public ActionResult MeterialAdd(Guid? id)
        {
            var c = db.Chapters.Find(id).CourseId;
            if (!isCourseCreator(c))
                return RedirectToAction("Index");

            ViewBag.ChapterId = id;
            return View();
        }


        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult MeterialAdd(Material material, Guid chapterId, IEnumerable<HttpPostedFileBase> uploadFiles)
        {
            var co = db.Chapters.Find(chapterId).CourseId;
            if (!isCourseCreator(co))
                return RedirectToAction("Index");

            if (ModelState.IsValid)
            {
                foreach (var item in uploadFiles)  //iterate in each file
                {
                    var fileName = "";

                    if (item != null && item.ContentLength > 0) //check length of bytes are greater then zero or not
                    {
                        fileName = Guid.NewGuid().ToString() + Path.GetExtension(item.FileName);
                        var imageFilePath = Path.Combine(Server.MapPath("~/Uploads"), fileName);
                        item.SaveAs(imageFilePath);
                        ViewBag.UploadSuccess = true;

                        //Add to DB
                        var d = new Document();
                        d.FilePath = "/ Uploads / " + fileName;
                        d.Owner = User.Identity.GetUserId();
                        d.DateUpload = DateTime.Now;
                        d.Description = item.FileName;

                        var doc = db.Documents.Add(d);

                        material.Documents.Add(doc);
                    }
                }

                material.Id = Guid.NewGuid();
                material.DateAdded = DateTime.Now;

                var c = db.Chapters.Find(chapterId);
                c.Materials.Add(material);

                db.Entry(c).State = EntityState.Modified;

                db.SaveChanges();

                return RedirectToAction("Chapters", new { id = c.CourseId });
            }
            return View(material);
        }

        public ActionResult MaterialEdit(Guid? id)
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
        public ActionResult MaterialEdit([Bind(Include = "Id,Title,Description,DateAdded")] Material material)
        {
            if (ModelState.IsValid)
            {
                db.Entry(material).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(material);
        }

        [Authorize]
        public ActionResult Grades(Guid? id)
        {
            if (id == null)
                return RedirectToAction("Index");

            var viewData = new CourseMembersViewModel();

            var course = db.Courses.Find(id);
            viewData.CourseInfo = course;

            var gradeTypes = db.GradeTypes.Where(g => g.Course.ID == course.ID).ToList();
            viewData.GradeTypes = gradeTypes;

            viewData.UserGrades = db.UserGrades.Include("GradeType").Where(c=>c.GradeType.Course.ID==course.ID).ToList();

            var courseUsers = db.UserCourses.Where(c => c.Course.ID == course.ID).ToList();

            viewData.Users = new List<ApplicationUser>();
            foreach (var item in courseUsers)
            {
                viewData.Users.Add(userDb.Users.Find(item.UserId));
            }

            viewData.isCreator = isCourseCreator(id);
            return View(viewData);
        }

        public  ActionResult CreateGradeType(Guid? Id)
        {
            if (Id == null)
                RedirectToAction("Index");
            ViewBag.CourseId = Id;
            return View(new GradeType());
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult CreateGradeType(GradeType model, Guid? courseId)
        {
            if (model != null || courseId !=null )
            {
                var course = db.Courses.FirstOrDefault(i => i.ID == courseId);
                model.Course = course;

                db.GradeTypes.Add(model);
                db.SaveChanges();

                return RedirectToAction("Grades", new { id = model.Course.ID });
            }

            return View();
        }


        public ActionResult GiveGrade(string userId, int? gradeTypeId)
        {
            if (userId == null || gradeTypeId == null)
                RedirectToAction("Index");

            ViewBag.Grade = 0;
            var gr = db.UserGrades.Where(g => g.GradeType.Id == gradeTypeId).FirstOrDefault(u => u.UserID == userId);
            if(gr!=null)
                ViewBag.Grade = gr.Grade;
            ViewBag.UserId = userId;
            ViewBag.GradeTypeId = gradeTypeId;
            return View();
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult GiveGrade(string userId, int? gradeTypeId, float grade)
        {
            if (userId != null && gradeTypeId != null)
            {
                var g = new UserGrade();
                g.UserID = userId;
                g.Grade = grade;
                g.GradeType = db.GradeTypes.FirstOrDefault(i => i.Id == gradeTypeId);

                db.UserGrades.Add(g);
                db.SaveChanges();

                return RedirectToAction("Grades", new { id = g.GradeType.Course.ID });
            }

            ViewBag.Grade = grade;
            ViewBag.UserId = userId;
            ViewBag.GradeTypeId = gradeTypeId;
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
            var m = db.Courses.Include("Discussion").Include("Discussion.Messages").Include("Discussion.Readings").FirstOrDefault(i => i.ID == id);

            ViewBag.CourseName = c.Name.ToUpper() + " - " + c.Description;
            ViewBag.CurrentUserId = User.Identity.GetUserId();
            return View(m);
        }

        [HttpPost]
        [Authorize]
        public ActionResult Read(Guid? DiscussionId, Guid? CourseId)
        {
            var c = db.Courses.Include("Discussion").Include("Discussion.Readings").FirstOrDefault(i => i.ID == CourseId);
            if (c == null)
                return Json(new { success = false });

            var d = c.Discussion.FirstOrDefault(i => i.ID == DiscussionId);
            if (d == null)
                return Json(new { success = false });

            var currentUser = User.Identity.GetUserId();
            var r = d.Readings.FirstOrDefault(u => u.UserId == currentUser);
            if (r == null)
            {
                r = new Discussion.Reading();
                r.UserId = currentUser;
                r.Date = DateTime.Now;
                d.Readings.Add(r);
            }
            else
            {
                r.Date = DateTime.Now;
            }

            db.Entry(c).State = EntityState.Modified;
            db.SaveChanges();

            return Json(new { success = true });
        }

        [Authorize]
        public ActionResult AddTitle(string id)
        {
            ViewBag.CourseId = id;
            return View();
        }

        [HttpPost]
        [Authorize]
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
        [Authorize]
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

        [Authorize]
        public ActionResult Join(Guid Id)
        {
            var userID = User.Identity.GetUserId(); ;
            var c = db.Courses.FirstOrDefault(i => i.ID == Id);

            var uc = db.UserCourses.Where(u => u.UserId == userID).FirstOrDefault(i => i.Course.ID == Id);

            if (uc == null)
            {
                uc = new UserCourse();
                uc.UserId = userID;

                uc.Course = c;
                uc.ApprovalDate = null;

                db.UserCourses.Add(uc);
                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        [Authorize]
        public ActionResult Leave(Guid Id)
        {
            var userID = User.Identity.GetUserId(); 
            var c = db.Courses.FirstOrDefault(i => i.ID == Id);

            var uc = db.UserCourses.Where(u => u.UserId == userID).FirstOrDefault(i => i.Course.ID == Id);

            if (uc != null)
            {
                db.UserCourses.Remove(uc);
                db.SaveChanges();
            }

            ViewBag.UserId = userID;
            return RedirectToAction("Index");
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