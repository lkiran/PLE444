using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using PLE444.Models;
using System.IO;
using PLE.Contract.Enums;
using PLE.Website.Service;
using PLE444.ViewModels;

namespace PLE444.Controllers
{
	public class MaterialController : Controller
	{
		#region Fields
		private PleDbContext db = new PleDbContext();
		private CourseService _courseService;
		#endregion

		#region Ctor
		public MaterialController() {
			_courseService = new CourseService();
		}
		#endregion

		[PleAuthorization]
		public ActionResult Index(Guid? id) {
			if (id == null)
				return RedirectToAction("Index", "Home");

			if (!isViewer(id) && !isMember(id) && !isCourseCreator(id))
				return RedirectToAction("Index", "Course", new { id });

			var chapters = db.Chapters.Where(c => c.Course.Id == id)
				.Select(e => new {
					e,
					Materials = e.Materials.Where(m => m.IsActive)
				})
				.AsEnumerable()
				.Select(e => e.e)
				.ToList();

			var course = db.Courses.FirstOrDefault(i => i.Id == id);

			var model = new CourseMaterials {
				CourseId = (Guid)id,
				CanEdit = isCourseCreator(id),
				ChapterList = chapters,
				IsItActive = course.IsCourseActive
			};
			return View(model);
		}

		[PleAuthorization]
		public ActionResult Add(Guid? chapterId) {
			if (chapterId == null)
				return RedirectToAction("Index");

			var courseId = db.Chapters.Find(chapterId).CourseId;
			if (!isCourseCreator(courseId))
				return RedirectToAction("Index");

			var model = new MaterialForm {
				ChapterId = (Guid)chapterId,
			};

			return View(model);
		}

		[HttpPost]
		[PleAuthorization]
		[ValidateAntiForgeryToken]
		public ActionResult Add(MaterialForm model) {
			var chapter = db.Chapters.Find(model.ChapterId);

			if (!isCourseCreator(chapter.CourseId))
				return RedirectToAction("Index");

			else if (ModelState.IsValid) {
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
						var d = new Document {
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

				var material = new Material {
					Id = Guid.NewGuid(),
					DateAdded = DateTime.Now,
					Description = model.Description,
					Documents = model.Documents,
					Title = model.Title
				};

				chapter.Materials.Add(material);
				db.Entry(chapter).State = EntityState.Modified;

				db.SaveChanges();

				return RedirectToAction("Index", "Chapter", new { id = chapter.CourseId });
			}
			return View(model);
		}

		[PleAuthorization]
		public ActionResult Update(Guid? chapterId, Guid? Id) {
			if (Id == null || chapterId == null)
				return RedirectToAction("Index");

			var courseId = db.Chapters.Find(chapterId).CourseId;
			if (!isCourseCreator(courseId))
				return RedirectToAction("Index");

			var material = db.Materials.SingleOrDefault(i => i.Id == Id);
			if (material == null)
				return RedirectToAction("Index");

			var model = new MaterialForm {
				Id = material.Id,
				ChapterId = (Guid)chapterId,
				Description = material.Description,
				Documents = material.Documents.ToList(),
				Title = material.Title
			};

			return View(model);
		}

		[HttpPost]
		[PleAuthorization]
		[ValidateAntiForgeryToken]
		public ActionResult Update(MaterialForm model) {
			var chapter = db.Chapters.Find(model.ChapterId);

			if (!isCourseCreator(chapter.CourseId))
				return RedirectToAction("Index");

			else if (ModelState.IsValid) {
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
						var d = new Document {
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

		public ActionResult Detail(Guid? id) {
			if (id == null)
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

			var material = db.Materials.Include(d => d.Documents).FirstOrDefault(m => m.Id == id);

			if (material == null)
				return HttpNotFound();

			return PartialView(material);
		}


		[HttpPost]
		[PleAuthorization]
		public JsonResult RemoveFromChapter(Guid? chapterId, Guid? materialId) {
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
		//[PleAuthorization]
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