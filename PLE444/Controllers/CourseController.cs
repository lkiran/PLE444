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
using WebGrease.Css.Extensions;

namespace PLE444.Controllers
{
    public class CourseController : Controller
    {
        private PleDbContext db = new PleDbContext();

        public ActionResult Index(Guid? id)
        {
            if (id == null)
                return RedirectToAction("List");

            var model = db.Courses.Include("Timeline").Include("Timeline.Creator").FirstOrDefault(c => c.Id == id);

            if (model == null)
                return HttpNotFound();

            return View(model);
        }

        public ActionResult List()
        {
            var c = db.Courses.ToList();
            var ui = User.Identity.GetUserId();
            var uc = db.UserCourses.Where(i => i.UserId == ui);
            var joinList = c.Select(item => uc.FirstOrDefault(i => i.Course.Id == item.Id)).Select(r => r != null).ToList();
            ViewBag.JoinList = joinList;
            return View(c);
        }

        [ChildActionOnly]
        public ActionResult Navigation(Guid? id)
        {
            var model = db.Courses.SingleOrDefault(i => i.Id == id);
            return PartialView(model);
        }

        [Authorize]
        public ActionResult Create()
        {
            ViewBag.Sapces = db.Spaces.ToList();
            return View();
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Course course)
        {
            if (ModelState.IsValid)
            {
                course.CreatorId = User.Identity.GetUserId();
                course.DateCreated= DateTime.Now;
                course.Timeline = new List<TimelineEntry>
                {
                    new TimelineEntry
                    {
                        Heading = "Ders oluşturuldu",
                        CreatorId = User.Identity.GetUserId(),
                        DateCreated = DateTime.Now,
                        IconClass = "ti ti-plus"
                    }
                };

                course = db.Courses.Add(course);
                db.SaveChanges();
                return RedirectToAction("Index", new {id = course.Id});
            }
            ViewBag.Sapces = db.Spaces.ToList();
            return View(course);
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

        [Authorize]
        public ActionResult Grades(Guid? courseId)
        {
            if (courseId == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var course = db.Courses.SingleOrDefault(i => i.Id == courseId);
            if (course == null)
                HttpNotFound();

            var courseUsers = db.UserCourses.Where(i => i.Course.Id == courseId).Include("User").ToList();

            var userIds = courseUsers.Select(u => u.UserId).ToList();
            var ug = db.UserGrades.Where(g=>userIds.Contains(g.UserId)).ToList();

            var model = new CourseGrades
            {
                CourseInfo = course,
                CanEdit = isCourseCreator(course),
                UserGrades = ug,
                GradeTypes = db.GradeTypes.Where(c => c.Course.Id == courseId).ToList(),
                CourseUsers = courseUsers
            };

            return View(model);
        }

        public  ActionResult CreateGradeType(Guid? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            ViewBag.CourseId = id;
            return View(new GradeType());
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult CreateGradeType(GradeType model, Guid? courseId)
        {
            if (model != null || courseId !=null )
            {
                var course = db.Courses.FirstOrDefault(i => i.Id == courseId);
                model.Course = course;

                db.GradeTypes.Add(model);
                db.SaveChanges();

                return RedirectToAction("Grades", new { courseId = model.Course.Id });
            }

            return View();
        }

        public ActionResult EditGradeType(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var model = db.GradeTypes.Find(id);
            if(model == null)
                HttpNotFound();

            return View(model);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult EditGradeType(GradeType model)
        {
            if (ModelState.IsValid)
            {
                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();

                var course = db.Courses.FirstOrDefault(i => i.Id == model.CourseId);
                return RedirectToAction("Grades", new {courseId = course.Id});
            }

            return View(model);
        }

        [Authorize]
        public ActionResult RemoveGradeType(int? id)
        {
            if (!id.HasValue)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var gradeType = db.GradeTypes.Find(id);
            if (gradeType == null)
                return HttpNotFound();

            else if(!isCourseCreator(gradeType.CourseId))
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            gradeType.IsActive = false;

            db.Entry(gradeType).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Grades", "Course", new { courseId = gradeType.CourseId});
        }

        [Authorize]
        public ActionResult ChangeGrade(int? gradeId)
        {
            if (gradeId == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var model = db.UserGrades.Where(i => i.Id == gradeId).Include("GradeType").Include("GradeType.Course").FirstOrDefault();
            if(model == null)
                return HttpNotFound();

            var course = model.GradeType.Course;
            if (!isCourseCreator(course))
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            return View(model);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult ChangeGrade(UserGrade model)
        {
            if (ModelState.IsValid)
            {
                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();

                var courseId = db.GradeTypes.Find(model.GradeTypeId).CourseId;
                return RedirectToAction("Grades", "Course", new { courseId = courseId });
            }
            return View(model);
        }

        [Authorize]
        public ActionResult AddGrade(int? gradeTypeId, string userId)
        {
            if (gradeTypeId == null || userId.IsNullOrWhiteSpace())
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var gradeType = db.GradeTypes.Where(i => i.Id == gradeTypeId).Include("Course").SingleOrDefault();

            if (gradeType != null && !isCourseCreator(gradeType.Course))
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);

            var model = new UserGrade
            {
                UserId = userId,
                GradeTypeId = (int)gradeTypeId
            };

            return View(model);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult AddGrade(UserGrade model)
        {
            if (ModelState.IsValid)
            {
                db.UserGrades.Add(model);
                db.SaveChanges();

                var courseId = db.GradeTypes.Find(model.GradeTypeId).CourseId;
                return RedirectToAction("Grades", "Course", new { courseId = courseId});
            }
            return View(model);
        }

        [Authorize]
        public JsonResult AddOrUpdateGradeJson(int gradeTypeId, string userId, float grade)
        {
            if(userId.IsNullOrWhiteSpace())
                return Json(new { Success = false, Message = "BadRequest" }, JsonRequestBehavior.AllowGet);

            var gt = db.GradeTypes.Include("Course").FirstOrDefault(i => i.Id == gradeTypeId);
            if(gt == null)
                return Json(new { Success = false, Message = "HttpNotFound" }, JsonRequestBehavior.AllowGet);

            if (!isCourseCreator(gt.Course))
                return Json(new { Success = false, Message = "Unauthorized" }, JsonRequestBehavior.AllowGet);

            var model = db.UserGrades.Where(u => u.UserId == userId).FirstOrDefault(t => t.GradeTypeId == gradeTypeId);
            if (model == null)
            {
                model = new UserGrade
                {
                    UserId = userId,
                    GradeTypeId = gradeTypeId,
                    Grade = grade
                };

                model = db.UserGrades.Add(model);
                db.SaveChanges();
            }
            else
            {
                model.Grade = grade;

                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();
            }

            return Json(new { Success = true, Message = "OK", ID = model.Id }, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public JsonResult DeleteGradeJson(int? id)
        {
            if (!id.HasValue)
                return Json(new { Success = false, Message = "BadRequest" }, JsonRequestBehavior.AllowGet);

            var userGrade = db.UserGrades.Find(id);
            if (userGrade == null)
                return Json(new { Success = false, Message = "HttpNotFound" }, JsonRequestBehavior.AllowGet);

            var gradeType = db.GradeTypes.Find(userGrade.GradeTypeId);
            if (gradeType == null)
                return Json(new { Success = false, Message = "HttpNotFound" }, JsonRequestBehavior.AllowGet);

            else if (!isCourseCreator(gradeType.CourseId))
                return Json(new { Success = false, Message = "Unauthorized" }, JsonRequestBehavior.AllowGet);

            db.UserGrades.Remove(userGrade);
            db.SaveChanges();

            return Json(new { Success = true, Message = "OK" }, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult DeleteGrade(int? id)
        {
            if (!id.HasValue)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var userGrade = db.UserGrades.Find(id);
            if (userGrade == null)
                return HttpNotFound();

            var gradeType = db.GradeTypes.Find(userGrade.GradeTypeId);
            if (gradeType == null)
                return HttpNotFound();

            else if (!isCourseCreator(gradeType.CourseId))
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            db.UserGrades.Remove(userGrade);
            db.SaveChanges();

            return RedirectToAction("Grades", "Course", new { courseId = gradeType.CourseId });
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
            var m = db.Courses.Include("Discussion").Include("Discussion.Messages").Include("Discussion.Readings").FirstOrDefault(i => i.Id == id);

            ViewBag.CourseName = c.Name.ToUpper() + " - " + c.Description;
            ViewBag.CurrentUserId = User.Identity.GetUserId();
            return View(m);
        }

        [HttpPost]
        [Authorize]
        public ActionResult Read(Guid? discussionId, Guid? courseId)
        {
            var c = db.Courses.Include("Discussion").Include("Discussion.Readings").FirstOrDefault(i => i.Id == courseId);
            if (c == null)
                return Json(new { success = false });

            var d = c.Discussion.FirstOrDefault(i => i.ID == discussionId);
            if (d == null)
                return Json(new { success = false });

            var currentUser = User.Identity.GetUserId();
            var r = d.Readings.FirstOrDefault(u => u.UserId == currentUser);
            if (r == null)
            {
                r = new Discussion.Reading
                {
                    UserId = currentUser,
                    Date = DateTime.Now
                };
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
        public ActionResult Join(Guid id)
        {
            var userID = User.Identity.GetUserId(); ;
            var c = db.Courses.FirstOrDefault(i => i.Id == id);

            var uc = db.UserCourses.Where(u => u.UserId == userID).FirstOrDefault(i => i.Course.Id == id);

            if (uc == null)
            {
                uc = new UserCourse();
                uc.UserId = userID;

                uc.Course = c;

                db.UserCourses.Add(uc);
                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        [Authorize]
        public ActionResult Leave(Guid id)
        {
            var userID = User.Identity.GetUserId(); 
            var c = db.Courses.FirstOrDefault(i => i.Id == id);

            var uc = db.UserCourses.Where(u => u.UserId == userID).FirstOrDefault(i => i.Course.Id == id);

            if (uc != null)
            {
                db.UserCourses.Remove(uc);
                db.SaveChanges();
            }

            ViewBag.UserId = userID;
            return RedirectToAction("Index");
        }

        [Authorize]
        [HttpPost]
        public ActionResult EjectUserFromCourse(string userId, Guid? courseId)
        {
            if(userId.IsNullOrWhiteSpace() || courseId == null)
                return Json(new { Success = false, Message = "BadRequest" }, JsonRequestBehavior.AllowGet);

            var uc = db.UserCourses.FirstOrDefault(c => c.CourseId == courseId && c.UserId == userId);
            if (uc == null)
                return Json(new { Success = false, Message = "HttpNotFound" }, JsonRequestBehavior.AllowGet);

            if (!isCourseCreator(uc.CourseId))
                return Json(new { Success = false, Message = "Unauthorized" }, JsonRequestBehavior.AllowGet);

            uc.IsActive = false;

            db.Entry(uc).State = EntityState.Modified;
            db.SaveChanges();

            return Json(new { Success = true, Message = "OK" }, JsonRequestBehavior.AllowGet);
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
            var user = db.UserCourses.Where(c => c.Course.Id == courseId).FirstOrDefault(u => u.UserId == userId);

            if (user == null)
                return false;
            return true;
        }

    }
}