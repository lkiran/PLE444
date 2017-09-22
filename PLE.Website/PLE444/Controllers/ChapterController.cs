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

		[Authorize]
		public ActionResult Index(Guid? id)
		{
			if (id == null)
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			else
			{
				var course = db.Courses.SingleOrDefault(i => i.Id == id);
				if (course == null)
					return HttpNotFound();

				if (!isMember(course) && !isCourseCreator(course))
					return RedirectToAction("Index", "Course", new { id = course.Id });

				var chapters = db.Chapters.Where(i => i.CourseId == id && i.IsActive)
					.OrderByDescending(c => c.OrderBy)
					.Include("Materials").ToList();

				var model = new Chapters
				{
					canEdit = isCourseCreator(course),
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
					OrderBy = chapter.OrderBy,
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
                chapterDb.OrderBy = model.OrderBy;

				db.Entry(chapterDb).State = EntityState.Modified;
				db.SaveChanges();

				return RedirectToAction("Index", new { id = model.CourseId });
			}

			return View(model);
		}

		[HttpPost]
		[Authorize]
		public JsonResult Delete(Guid? id)
		{
			if (id == null)
				return Json(new { Success = false, Message = "BadRequest" }, JsonRequestBehavior.AllowGet);

			var chapter = db.Chapters.Find(id);
			if (chapter == null)
				return Json(new { Success = false, Message = "HttpNotFound" }, JsonRequestBehavior.AllowGet);

			else if (!isCourseCreator(chapter.CourseId))
				return Json(new { Success = false, Message = "Unauthorized" }, JsonRequestBehavior.AllowGet);

			chapter.IsActive = false;

			db.Entry(chapter).State = EntityState.Modified;
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

			else if (course.CreatorId != User.GetPrincipal()?.User.Id)
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
	}
}