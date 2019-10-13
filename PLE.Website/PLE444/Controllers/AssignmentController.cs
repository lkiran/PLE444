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
using System.Net.Mail;
using System.Threading.Tasks;
using PLE.Contract.Enums;
using PLE.Website.Service;
using static PLE444.Helpers.ViewHelper;

namespace PLE444.Controllers {
	[PleAuthorization]
	public class AssignmentController : Controller {
		#region Fields
		private PleDbContext db = new PleDbContext();
		private CourseService _courseService;
		private EmailService ms = new EmailService();
		#endregion

		#region Ctor
		public AssignmentController() {
			_courseService = new CourseService();
		} 
		#endregion

		[PleAuthorization]
		public ActionResult Index(Guid? id) {
			if (id == null)
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

			var course = db.Courses.Find(id);
			if (course == null)
				return HttpNotFound();

			if (!isViewer(id) && !isMember(id) && !isCourseCreator(course))
				return RedirectToAction("Index", "Course", new { id = id });

			var model = new CourseAssignments {
				CourseInfo = course,
				CanEdit = isCourseCreator(course),
				CanUpload = isMember(course.Id),
				CurrentUserId = User.GetPrincipal()?.User.Id
			};

            if (!isCourseCreator(course))
                model.AssignmentList = db.Assignments.Where(i => i.CourseId == id && i.IsActive && !i.IsHidden).Include("Uploads").ToList();
            else

                model.AssignmentList = db.Assignments.Where(i => i.CourseId == id && i.IsActive).Include("Uploads").Include("Uploads.Owner").ToList();

            return View(model);
        }

		public ActionResult Create(Guid id) {
			if (!isCourseCreator(id))
				return RedirectToAction("Index", "Home");

			var model = new AssignmentForm {
				CourseId = id,
				Deadline = DateTime.Now
			};
			return View(model);
		}

		[HttpPost]
		[PleAuthorization]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Create(AssignmentForm model) {
			var course = db.Courses.Find(model.CourseId);

			if (!course.IsCourseActive)
				return RedirectToAction("Index", "Home");

			if (!isCourseCreator(course))
				return RedirectToAction("Index", "Home");

			else if (ModelState.IsValid) {
				var assignment = new Assignment {
					DateAdded = DateTime.Now,
					Title = model.Title,
					Description = model.Description,
					Deadline = model.Deadline,
                    IsHidden=model.IsHidden
         
				};
				db.Assignments.Add(assignment);

				course.Assignments.Add(assignment);
				db.Entry(course).State = EntityState.Modified;

				db.SaveChanges();

				var joinedUsers = db.UserCourses
					.Where(uc => uc.CourseId == model.CourseId && uc.IsActive && uc.DateJoin != null)
					.Include(uc => uc.User)
					.Select(uc => uc.User).ToList();
				List<string> emails = new List<string>();
				foreach (var item in joinedUsers) {
					emails.Add(item.Email);
				}

				//Send email to participants if there any
				if (emails != null || emails.Any()) {
					try {
						var mail = new MailMessage() {
							Subject = course.Heading + " dersine " + model.Title + " ödevi eklendi.",
							Body = ViewRenderer.RenderView("~/Views/Mail/NewAssignment.cshtml", new ViewDataDictionary()
						{
							{"title", model.Title},
							{"deadline", model.Deadline},
							{"description", model.Description},
							{"course", course.Heading},
							{"courseId", assignment.Id}
						})
						};

						mail.IsBodyHtml = true;
						foreach (var receiver in emails.ToList())
							mail.Bcc.Add(receiver);

						await ms.SendAsync(mail);
					}
					catch(Exception ex) {
						Console.WriteLine(ex.ToString());
					}
					
				}

				return RedirectToAction("Index", "Assignment", new { id = model.CourseId });
			}

			return View(model);
		}

		public ActionResult Edit(Guid? id, Guid? courseId) {
			if (!id.HasValue || !courseId.HasValue)
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

			if (!isCourseCreator(courseId))
				return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);

			var assignment = db.Assignments.Include("Course").FirstOrDefault(i => i.Id == id);
			if (assignment == null)
				return HttpNotFound();

			var model = new AssignmentForm {
				Id = assignment.Id,
				CourseId = assignment.CourseId,
				Deadline = assignment.Deadline,
				Description = assignment.Description,
				Title = assignment.Title
			};

			return View(model);
		}

		[HttpPost]
		[PleAuthorization]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(AssignmentForm model) {
			if (ModelState.IsValid) {
				var assignment = db.Assignments.Include("Course").SingleOrDefault(i => i.Id == model.Id);
				if (assignment == null)
					return HttpNotFound();

				if (!isCourseCreator(assignment.Course))
					return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);

				assignment.Deadline = model.Deadline;
				assignment.Description = model.Description;
				assignment.Title = model.Title;
                assignment.IsHidden = model.IsHidden;
				db.Entry(assignment).State = EntityState.Modified;
				db.SaveChanges();

				return RedirectToAction("Index", "Assignment", new { id = assignment.CourseId });
			}

			return View(model);
		}

		[HttpPost]
		[PleAuthorization]
		public ActionResult Delete(Guid? id) {
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
		[PleAuthorization]
		[ValidateAntiForgeryToken]
		public ActionResult Feedback(int? uploadId, string feedback) {
			if (uploadId == null)
				return Json(new { Success = false, Message = "BadRequest" }, JsonRequestBehavior.AllowGet);

			var document = db.Documents.Find(uploadId);
			if (document == null)
				return Json(new { Success = false, Message = "HttpNotFound" }, JsonRequestBehavior.AllowGet);

			document.Feedback = feedback;

			db.Entry(document).State = EntityState.Modified;
			db.SaveChanges();

			return Json(new { Success = true, Message = "OK" }, JsonRequestBehavior.AllowGet);
		}

		[PleAuthorization]
		public ActionResult Publish(Guid? assignmentId, Guid? courseId) {
			var assignment = db.Assignments.Include("Course").SingleOrDefault(i => i.Id == assignmentId);

			if (assignment == null)
				return HttpNotFound();

			if (assignment.IsFeedbackPublished == true) {
				assignment.IsFeedbackPublished = false;
			} else {
				assignment.IsFeedbackPublished = true;
			}

			db.Entry(assignment).State = EntityState.Modified;
			db.SaveChanges();
			return RedirectToAction("Index", "Assignment", new { id = assignment.CourseId });
		}

		[HttpPost]
		[PleAuthorization]
		[ValidateAntiForgeryToken]
		public ActionResult Upload(Guid assignmentId, HttpPostedFileBase uploadFile) {
			var a = db.Assignments.Include("Course").FirstOrDefault(i => i.Id == assignmentId);

			if (a == null || !isMember(a.Course.Id))
				return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);

			if (ModelState.IsValid) {
				var currentuserId = User.GetPrincipal()?.User.Id;

				if (uploadFile != null && uploadFile.ContentLength > 0) {
					var filePath = "";
					var fileName = "";

					fileName = Guid.NewGuid().ToString() + Path.GetExtension(uploadFile.FileName);
					filePath = Path.Combine(Server.MapPath("~/Uploads"), fileName);
					uploadFile.SaveAs(filePath);
					ViewBag.UploadSuccess = true;

					var uploaded = a.Uploads.FirstOrDefault(u => u.OwnerId == currentuserId);
					if (uploaded == null) {
						var d = new Document {
							DateUpload = DateTime.Now,
							Description = uploadFile.FileName,
							OwnerId = currentuserId,
							FilePath = "~/Uploads/" + fileName
						};

						db.Documents.Add(d);
						a.Uploads.Add(d);
					} else {
						uploaded.DateUpload = DateTime.Now;
						uploaded.Description = uploadFile.FileName;
						uploaded.FilePath = "~/Uploads/" + fileName;
						uploaded.OwnerId = currentuserId;

						db.Entry(uploaded).State = EntityState.Modified;
					}

					db.Entry(a).State = EntityState.Modified;
					db.SaveChanges();
				}
			}

			return RedirectToAction("Index", "Assignment", new { id = a.Course.Id });
		}

		[PleAuthorization]
		public ActionResult DownloadAssignment(Guid asssignmentId) {
			var assignment = db.Assignments.Include("Uploads").Include("Uploads.Owner").FirstOrDefault(a => a.Id == asssignmentId);

			if (assignment == null)
				return HttpNotFound();

			if (!isCourseCreator(assignment.CourseId))
				return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);

			var documents = assignment.Uploads.ToList();

			var memoryStream = new MemoryStream();

			using (var zip = new ZipFile()) {
				foreach (var document in documents) {
					var path = Server.MapPath(document.FilePath);

					if (!System.IO.File.Exists(path)) continue;
					var zipFileName = document.Owner.FullName() + "_" + document.Description;
					zip.AddFile(path, "").FileName = zipFileName;
				}
				zip.Save(memoryStream);
			}
			memoryStream.Seek(0, 0);
			return File(memoryStream, "application/octet-stream", DateTime.Now + ".zip");
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
		#endregion
	}
}