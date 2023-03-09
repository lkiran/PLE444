using System;
using System.Net;
using System.Linq;
using PLE444.Models;
using System.Web.Mvc;
using PLE444.ViewModels;
using System.Data.Entity;
using PLE.Contract.Enums;
using PLE.Website.Service;

namespace PLE444.Controllers
{
	public class ChapterController : Controller
	{
		#region Fields
		private PleDbContext db = new PleDbContext();
		private CourseService _courseService;
		#endregion

		#region Ctor
		public ChapterController() {
			_courseService = new CourseService();
		} 

		#endregion
	
		[PleAuthorization]
		public ActionResult Index(Guid? id) {
			if (id == null)
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			else {
				var course = db.Courses.SingleOrDefault(i => i.Id == id);
				if (course == null)
					return HttpNotFound();

				if (!isViewer(course) && !isMember(course) && !isCourseCreator(course))
					return RedirectToAction("Index", "Course", new { id = course.Id });

				var model = new Chapters {
					canEdit = isCourseCreator(course),
					CourseInfo = course,
					ChapterList = db.Chapters
						.OrderByDescending(c => c.OrderBy).ThenBy(x => x.DateAdded)
						.Include("Materials"),
				};
				model.ChapterList = !isCourseCreator(course)
					? model.ChapterList.Where(i => i.CourseId == id && i.IsActive && !i.IsHidden)
					: model.ChapterList.Where(i => i.CourseId == id && i.IsActive);

				return View(model);
			}
		}

		[PleAuthorization]
		public ActionResult Create(Guid id) {
			if (!isCourseCreator(id))
				return RedirectToAction("Index", "Home");

			var model = new Chapter {
				CourseId = id
			};
			return View(model);
		}

		[HttpPost]
		[PleAuthorization]
		[ValidateAntiForgeryToken]
		public ActionResult Create(Chapter chapter) {
			if (!isCourseCreator(chapter.CourseId))
				return RedirectToAction("Index", "Home");

			else if (ModelState.IsValid) {
				var c = new Chapter {
					DateAdded = DateTime.Now,
					Title = chapter.Title,
					OrderBy = chapter.OrderBy,
					Description = chapter.Description,
					IsHidden = chapter.IsHidden
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

		[PleAuthorization]
		public ActionResult Edit(Guid? id) {
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
		[PleAuthorization]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(Chapter model) {
			if (ModelState.IsValid) {
				if (!isCourseCreator(model.CourseId))
					return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);

				var chapterDb = db.Chapters.Find(model.Id);
				chapterDb.Description = model.Description;
				chapterDb.Title = model.Title;
				chapterDb.OrderBy = model.OrderBy;
				chapterDb.IsHidden = model.IsHidden;
				db.Entry(chapterDb).State = EntityState.Modified;
				db.SaveChanges();

				return RedirectToAction("Index", new { id = model.CourseId });
			}

			return View(model);
		}

		[HttpPost]
		[PleAuthorization]
		public JsonResult Delete(Guid? id) {
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
		
		#region Private Methods
		private bool isCourseCreator(Guid? courseId) {
			if (courseId == null)
				return false;
			var identity = User.GetPrincipal()?.Identity as PleClaimsIdentity;
			if (identity == null)
				return false;
			return identity.HasClaim(PleClaimType.Creator, courseId.ToString());
		}

		private bool isCourseCreator(Course course) {
			return isCourseCreator(course.Id);
		}

		private bool isMember(Guid? courseId) {
			if (courseId == null)
				return false;
			if (!(User.GetPrincipal()?.Identity is PleClaimsIdentity identity))
				return false;
			return identity.HasClaim(PleClaimType.Member, courseId.ToString());
		}

		private bool isMember(Course course) {
			return isMember(course.Id);
		}

		private bool isViewer(Course course) {
			return isViewer(course.Id);
		}

		private bool isViewer(Guid? courseId) {
			if (courseId == null)
				return false;
			if (!(User.GetPrincipal()?.Identity is PleClaimsIdentity identity))
				return false;
			return identity.HasClaim(PleClaimType.Viewer, courseId.ToString());
		}

		private bool isWaiting(Guid? courseId) {
			if (courseId == null)
				return false;
			if (!(User.GetPrincipal()?.Identity is PleClaimsIdentity identity))
				return false;
			var waiting = identity.HasClaim(PleClaimType.Waiting, courseId.ToString());
			if (!waiting)
				return waiting;
			identity.AddClaims(_courseService.GetClaims());
			waiting = identity.HasClaim(PleClaimType.Waiting, courseId.ToString());
			return waiting;
		}

		protected override void Dispose(bool disposing) {
			if (disposing) {
				db.Dispose();
			}
			base.Dispose(disposing);
		}
		#endregion
	}
}