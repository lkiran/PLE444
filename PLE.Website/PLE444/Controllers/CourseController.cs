﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Web.Mvc;
using AutoMapper;
using PLE444.Models;
using Microsoft.AspNet.Identity;
using Microsoft.Ajax.Utilities;
using PLE.Contract.DTOs;
using PLE.Contract.DTOs.Requests;
using PLE.Contract.Enums;
using PLE.Website.Service;
using PLE444.ViewModels;

namespace PLE444.Controllers
{
	public class CourseController : Controller
	{
		#region Fields
		private PleDbContext db = new PleDbContext();
		private CourseService _courseService;
		#endregion

		#region Ctor
		public CourseController() {
			_courseService = new CourseService();
		}
		#endregion

		#region Course
		public ActionResult Index(Guid? id) {
			if (id == null)
				return RedirectToAction("List");

			var course = _courseService.Detail(id.Value);

			if (course == null)
				return HttpNotFound();
			course.Timeline = course.Timeline.OrderByDescending(e => e.DateCreated).ToList();

			var model = new CourseViewModel {
				Course = course,
				IsCourseCreator = isCourseCreator(course.Id),
				IsMember = isMember(course.Id),
				IsWaiting = isWaiting(course.Id),
			};
			return View(model);
		}

		public ActionResult List() {
			var model = db.Courses.Where(c => c.CanEveryoneJoin && c.IsCourseActive).ToList();
			return View(model);
		}


		[ChildActionOnly]
		public ActionResult Navigation(Guid? id) {
			var model = db.Courses.SingleOrDefault(i => i.Id == id);
			return PartialView(model);
		}

		[PleAuthorization]
		public ActionResult Create() {
			return View();
		}

		[HttpPost]
		[PleAuthorization]
		[ValidateAntiForgeryToken]
		public ActionResult Create(CreateCourseViewModel course) {
			try {
				if (!ModelState.IsValid)
					return View(course);

				var request = Mapper.Map<CourseDto>(course);
				var id = _courseService.Create(request);

				#region Add claim
				var identity = User.GetPrincipal()?.Identity as PleClaimsIdentity;
				identity?.AddClaim(new Claim(PleClaimType.Creator, id.ToString()));
				#endregion

				return RedirectToAction("Index", new { id });
			} catch (Exception e) {
				Console.Write(e.Message);
				return View(course);
			}
		}

		[HttpPost]
		[PleAuthorization]
		[ValidateAntiForgeryToken]
		public ActionResult CreateDuplicate(Course model) {
			if (!ModelState.IsValid)
				return View("Edit", model);

			if (!isCourseCreator(model.Id))
				return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);

			var request = new DuplicateCourseRequestDto {
				Id = model.Id,
				NewCode = model.Code,
				NewName = model.Name
			};
			var newCourseId = _courseService.Duplicate(request);

			var identity = User.GetPrincipal()?.Identity as PleClaimsIdentity;
			identity?.AddClaim(new Claim(PleClaimType.Creator, newCourseId.ToString()));

			return RedirectToAction("Index", new { id = newCourseId });
		}

		[PleAuthorization]
		public ActionResult Edit(Guid? id) {
			if (id == null)
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

			var model = db.Courses.FirstOrDefault(c => c.Id == id);
			if (model == null)
				return HttpNotFound();

			if (!isCourseCreator(model))
				return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);

			return View(model);
		}

		[HttpPost]
		[PleAuthorization]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(Course model) {
			if (ModelState.IsValid) {
				var course = db.Courses.FirstOrDefault(c => c.Id == model.Id);

				if (course == null)
					return HttpNotFound();

				if (!isCourseCreator(course))
					return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);

				course.CanEveryoneJoin = model.CanEveryoneJoin;
				course.Code = model.Code;
				course.Name = model.Name;
				course.Description = model.Description;
				course.Timeline.Add(new TimelineEntry {
					ColorClass = "timeline-primary",
					CreatorId = course.CreatorId,
					DateCreated = DateTime.Now,
					IconClass = "ti ti-pencil",
					Heading = "Ders bilgileri değiştirildi"
				});

				db.Entry(course).State = EntityState.Modified;
				db.SaveChanges();
				return RedirectToAction("Index", new { id = model.Id });

			}
			return View(model);
		}
		#endregion

		#region Grades
		[PleAuthorization]
		public ActionResult Grades(Guid? courseId) {
			if (courseId == null)
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

			var course = db.Courses.SingleOrDefault(i => i.Id == courseId);
			if (course == null)
				HttpNotFound();

			if (!isMember(course) && !isCourseCreator(course))
				return RedirectToAction("Index", "Course", new { id = courseId });

			var courseUsers = db.UserCourses.Where(i => i.Course.Id == courseId).Include("User").ToList();

			var userIds = courseUsers.Select(u => u.UserId).ToList();
			var ug = db.UserGrades.Where(g => userIds.Contains(g.UserId)).ToList();

			var model = new CourseGrades {
				CourseInfo = course,
				CurrentUserId = User.Identity.GetUserId(),
				UserGrades = ug,
				GradeTypes = db.GradeTypes.Where(c => c.Course.Id == courseId).ToList(),
				CourseUsers = courseUsers
			};

			if (isCourseCreator(course))
				return View("GradesForEditor", model);
			else
				return View("GradesForMember", model);
		}

		[PleAuthorization]
		public ActionResult CreateGradeType(Guid? id) {
			if (id == null)
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			var course = db.Courses.Find(id);

			if (course == null)
				return HttpNotFound();

			if (!isCourseCreator(course))
				return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);

			ViewBag.CourseId = id;
			return View(new GradeType());
		}

		[HttpPost]
		[PleAuthorization]
		[ValidateAntiForgeryToken]
		public ActionResult CreateGradeType(GradeType model, Guid? courseId) {
			if (model != null || courseId != null) {
				var course = db.Courses.FirstOrDefault(i => i.Id == courseId);

				if (course == null)
					return HttpNotFound();

				if (!isCourseCreator(course))
					return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);

				model.Course = course;

				db.GradeTypes.Add(model);

				course.Timeline.Add(new TimelineEntry {
					ColorClass = "timeline-primary",
					CreatorId = course.CreatorId,
					DateCreated = DateTime.Now,
					IconClass = "ti ti-plus",
					Heading = "Not eklendi",
					Content = model.Name + " isminde, %" + model.Effect + " ortalamaya etkisi olan " + model.Description + " notu eklendi."
				});

				db.Entry(course).State = EntityState.Modified;
				db.SaveChanges();

				return RedirectToAction("Grades", new { courseId = model.Course.Id });
			}

			return View();
		}

		[PleAuthorization]
		public ActionResult EditGradeType(int? id) {
			if (id == null)
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

			var model = db.GradeTypes.Find(id);

			if (model == null)
				return HttpNotFound();

			if (!isCourseCreator(model.Course))
				return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);

			return View(model);
		}

		[HttpPost]
		[PleAuthorization]
		[ValidateAntiForgeryToken]
		public ActionResult EditGradeType(GradeType model) {
			if (ModelState.IsValid) {
				var gradeType = db.GradeTypes.Find(model.Id);

				if (gradeType == null)
					return HttpNotFound();

				if (!isCourseCreator(gradeType.Course))
					return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);

				gradeType.Effect = model.Effect;
				gradeType.Description = model.Description;
				gradeType.Name = model.Name;
				gradeType.MaxScore = model.MaxScore;

				gradeType.Course.Timeline.Add(new TimelineEntry {
					ColorClass = "timeline-warning",
					CreatorId = gradeType.Course.CreatorId,
					DateCreated = DateTime.Now,
					IconClass = "ti ti-pencil",
					Heading = model.Name + " notu değiştirildi"
				});

				db.Entry(gradeType).State = EntityState.Modified;
				db.SaveChanges();

				var course = db.Courses.FirstOrDefault(i => i.Id == model.CourseId);
				return RedirectToAction("Grades", new { courseId = course.Id });
			}

			return View(model);
		}

		[PleAuthorization]
		public ActionResult RemoveGradeType(int? id) {
			if (!id.HasValue)
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

			var gradeType = db.GradeTypes.Find(id);
			if (gradeType == null)
				return HttpNotFound();

			else if (!isCourseCreator(gradeType.CourseId))
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

			gradeType.IsActive = false;

			gradeType.Course.Timeline.Add(new TimelineEntry {
				ColorClass = "timeline-danger",
				CreatorId = gradeType.Course.CreatorId,
				DateCreated = DateTime.Now,
				IconClass = "ti ti-trash",
				Heading = gradeType.Name + " notu silindi"
			});

			db.Entry(gradeType).State = EntityState.Modified;
			db.SaveChanges();

			return RedirectToAction("Grades", "Course", new { courseId = gradeType.CourseId });
		}

		[PleAuthorization]
		public ActionResult ChangeGrade(int? gradeId) {
			if (gradeId == null)
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

			var model = db.UserGrades.Where(i => i.Id == gradeId).Include("GradeType").Include("GradeType.Course").FirstOrDefault();
			if (model == null)
				return HttpNotFound();

			var course = model.GradeType.Course;
			if (!isCourseCreator(course))
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

			return View(model);
		}

		[HttpPost]
		[PleAuthorization]
		[ValidateAntiForgeryToken]
		public ActionResult ChangeGrade(UserGrade model) {
			if (ModelState.IsValid) {
				db.Entry(model).State = EntityState.Modified;
				db.SaveChanges();

				var courseId = db.GradeTypes.Find(model.GradeTypeId).CourseId;
				return RedirectToAction("Grades", "Course", new { courseId = courseId });
			}
			return View(model);
		}

		[PleAuthorization]
		public ActionResult AddGrade(int? gradeTypeId, string userId) {
			if (gradeTypeId == null || userId.IsNullOrWhiteSpace())
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

			var gradeType = db.GradeTypes.Where(i => i.Id == gradeTypeId).Include("Course").SingleOrDefault();

			if (gradeType != null && !isCourseCreator(gradeType.Course))
				return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);

			var model = new UserGrade {
				UserId = userId,
				GradeTypeId = (int)gradeTypeId
			};

			return View(model);
		}

		[HttpPost]
		[PleAuthorization]
		[ValidateAntiForgeryToken]
		public ActionResult AddGrade(UserGrade model) {
			if (ModelState.IsValid) {
				db.UserGrades.Add(model);
				db.SaveChanges();

				var courseId = db.GradeTypes.Find(model.GradeTypeId).CourseId;
				return RedirectToAction("Grades", "Course", new { courseId = courseId });
			}
			return View(model);
		}

		[PleAuthorization]
		public JsonResult AddOrUpdateGradeJson(int gradeTypeId, string userId, float grade) {
			if (userId.IsNullOrWhiteSpace())
				return Json(new { Success = false, Message = "BadRequest" }, JsonRequestBehavior.AllowGet);

			var gt = db.GradeTypes.Include("Course").FirstOrDefault(i => i.Id == gradeTypeId);
			if (gt == null)
				return Json(new { Success = false, Message = "HttpNotFound" }, JsonRequestBehavior.AllowGet);

			if (!isCourseCreator(gt.Course))
				return Json(new { Success = false, Message = "Unauthorized" }, JsonRequestBehavior.AllowGet);

			var model = db.UserGrades.Where(u => u.UserId == userId).FirstOrDefault(t => t.GradeTypeId == gradeTypeId);
			if (model == null) {
				model = new UserGrade {
					UserId = userId,
					GradeTypeId = gradeTypeId,
					Grade = grade
				};

				model = db.UserGrades.Add(model);
				db.SaveChanges();
			} else {
				model.Grade = grade;

				db.Entry(model).State = EntityState.Modified;
				db.SaveChanges();
			}

			return Json(new { Success = true, Message = "OK", ID = model.Id }, JsonRequestBehavior.AllowGet);
		}

		[PleAuthorization]
		public JsonResult DeleteGradeJson(int? id) {
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

		[PleAuthorization]
		public ActionResult DeleteGrade(int? id) {
			if (!id.HasValue)
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

			var userGrade = db.UserGrades.Find(id);
			if (userGrade == null)
				return HttpNotFound();

			var gradeType = db.GradeTypes.Find(userGrade.GradeTypeId);
			if (gradeType == null)
				return HttpNotFound();

			else if (!isCourseCreator(gradeType.CourseId))
				return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);

			db.UserGrades.Remove(userGrade);
			db.SaveChanges();

			return RedirectToAction("Grades", "Course", new { courseId = gradeType.CourseId });
		}

		#endregion

		#region Discussions
		[PleAuthorization]
		public ActionResult Discussion(Guid? id) {
			var course = db.Courses.Include("Discussion")
				.Include("Creator")
				.Include("Discussion.Messages")
				.Include("Discussion.Messages.Sender")
				.Include("Discussion.Readings")
				.FirstOrDefault(i => i.Id == id);

			if (course == null)
				return HttpNotFound();

			if (!isMember(course) && !isCourseCreator(course))
				return RedirectToAction("Index", "Course", new { id = id });

			var model = new DiscussionViewModel {
				CId = course.Id,
				CurrentUserId = User.Identity?.GetUserId(),
				Role = isCourseCreator(course) ? "Creator" : "Member",
				Discussion = course.Discussion.ToList(),
				IsActive = course.IsCourseActive

			};

			return View(model);
		}

		#region Title
		[PleAuthorization]
		public ActionResult AddTitle(string id) {
			ViewBag.CourseId = id;
			return View();
		}

		[HttpPost]
		[PleAuthorization]
		[ValidateAntiForgeryToken]
		public ActionResult AddTitle(Discussion discussion, Guid courseId) {
			if (ModelState.IsValid) {
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

		[PleAuthorization]
		public ActionResult RemoveTitle(Guid? discussionId, Guid? CId) {
			if (CId == null || discussionId == null)
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

			var course = db.Courses.FirstOrDefault(c => c.Id == CId);
			if (course == null)
				return HttpNotFound();

			else if (!isCourseCreator(course))
				return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);

			var discussion = db.Discussions.FirstOrDefault(d => d.ID == discussionId);
			if (discussion == null)
				return HttpNotFound();

			db.Readings.RemoveRange(discussion.Readings);
			db.Messages.RemoveRange(discussion.Messages);
			db.Discussions.Remove(discussion);
			db.SaveChanges();

			return Redirect(HttpContext.Request.UrlReferrer.AbsoluteUri);
		}
		#endregion

		#region Messages
		[HttpPost]
		[PleAuthorization]
		public ActionResult Read(Guid? discussionId, Guid? CId) {
			if (discussionId == Guid.Empty || CId == Guid.Empty)
				return HttpNotFound();
			var c = db.Courses.Include("Discussion")
				.Include("Discussion.Messages.Sender")
				.Include("Discussion.Messages.Replies.Sender")
				.Include("Discussion.Readings")
				.FirstOrDefault(i => i.Id == CId);
			if (c == null)
				return Json(new { success = false });

			var d = c.Discussion.FirstOrDefault(i => i.ID == discussionId);
			if (d == null)
				return Json(new { success = false });

			var currentUser = User.Identity.GetUserId();
			var r = d.Readings.FirstOrDefault(u => u.UserId == currentUser);
			if (r == null) {
				r = new Discussion.Reading {
					UserId = currentUser,
					Date = DateTime.Now
				};
				d.Readings.Add(r);
			} else {
				r.Date = DateTime.Now;
			}

			db.Entry(c).State = EntityState.Modified;
			db.SaveChanges();

			var course = db.Courses.FirstOrDefault(i => i.Id == CId);

			var model = new DiscussionMessages {
				Discussion = d,
				CurrentUserId = currentUser,
				CId = (Guid)CId,
				Role = isCourseCreator(c) ? "Creator" : "Member",
				isActive = course.IsCourseActive
			};
			return PartialView(model);
		}

		[HttpPost]
		[PleAuthorization]
		[ValidateAntiForgeryToken]
		//public ActionResult SendMessage(SendMessageParametersVm message)
		public ActionResult SendMessage(NewMessageViewModel model) {
			if (!ModelState.IsValid)
				return View(model);

			var newMessage = new Message {
				Content = model.Content,
				DateSent = DateTime.Now,
				SenderId = User.Identity.GetUserId()
			};

			if (model.ReplyId != Guid.Empty) {
				var parentMessage = db.Messages.Find(model.ReplyId);
				parentMessage?.Replies.Add(newMessage);

				db.Entry(parentMessage).State = EntityState.Modified;
			} else {
				db.Messages.Add(newMessage);

				var discussion = db.Discussions.Find(model.DiscussionId);
				discussion?.Messages.Add(newMessage);

				db.Entry(discussion).State = EntityState.Modified;
			}

			db.SaveChanges();

			TempData["Active"] = model.DiscussionId;
			return RedirectToAction("Discussion", new { id = model.CId });
		}

		[PleAuthorization]
		public ActionResult RemoveMessage(Guid? messageId, Guid? CId) {
			if (messageId == null)
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

			try {
				var message = db.Messages.Include("Replies").FirstOrDefault(m => m.ID == messageId);
				if (message == null)
					return HttpNotFound();

				else if (!isCourseCreator(CId) && message.SenderId != User.GetPrincipal()?.User.Id)
					return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
				foreach (var reply in message.Replies.ToList())
					db.Messages.Remove(reply);


				db.Messages.Remove(message);
				db.SaveChanges();
			} catch (Exception e) { }

			return Redirect(HttpContext.Request.UrlReferrer.AbsoluteUri);
		}
		#endregion

		#endregion

		#region Members
		[PleAuthorization]
		public ActionResult Members(Guid? id) {
			if (id == null)
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

			var course = db.Courses.Find(id);
			if (course == null)
				return HttpNotFound();

			if (!isMember(course) && !isCourseCreator(course))
				return RedirectToAction("Index", "Course", new { id = course.Id });

			var model = new CourseMembers {
				Members = db.UserCourses.Include("User").Where(uc => uc.CourseId == id).ToList(),
				Course = course,
				CanEdit = isCourseCreator(course)
			};

			return View(model);
		}

		[PleAuthorization]
		public ActionResult Join(Guid id) {
			var userID = User.GetPrincipal()?.User.Id; ;
			var c = db.Courses.FirstOrDefault(i => i.Id == id);

			var uc = db.UserCourses.Where(u => u.UserId == userID).FirstOrDefault(i => i.Course.Id == id);

			if (uc == null) {
				uc = new UserCourse();
				uc.UserId = userID;
				uc.Course = c;

				db.UserCourses.Add(uc);
			} else {
				uc.IsActive = true;
				db.Entry(uc).State = EntityState.Modified;
			}

			db.SaveChanges();
			return RedirectToAction("Index", new { id = id });
		}

		[PleAuthorization]
		public ActionResult Leave(Guid id) {
			var userID = User.GetPrincipal()?.User.Id;
			var c = db.Courses.FirstOrDefault(i => i.Id == id);

			var uc = db.UserCourses.Where(u => u.UserId == userID).FirstOrDefault(i => i.Course.Id == id);

			if (uc == null)
				return HttpNotFound();

			uc.IsActive = false;
			uc.DateJoin = null;

			db.Entry(uc).State = EntityState.Modified;
			db.SaveChanges();


			return RedirectToAction("Index", new { id = id });
		}

		[PleAuthorization]
		[HttpPost]
		public ActionResult EjectUserFromCourse(string userId, Guid? courseId) {
			if (userId.IsNullOrWhiteSpace() || courseId == null)
				return Json(new { Success = false, Message = "BadRequest" }, JsonRequestBehavior.AllowGet);

			var uc = db.UserCourses.FirstOrDefault(c => c.CourseId == courseId && c.UserId == userId);
			if (uc == null)
				return Json(new { Success = false, Message = "HttpNotFound" }, JsonRequestBehavior.AllowGet);

			if (!isCourseCreator(uc.CourseId))
				return Json(new { Success = false, Message = "Unauthorized" }, JsonRequestBehavior.AllowGet);

			uc.IsActive = false;
			uc.DateJoin = null;

			db.Entry(uc).State = EntityState.Modified;
			db.SaveChanges();

			return Json(new { Success = true, Message = "OK" }, JsonRequestBehavior.AllowGet);
		}

		[PleAuthorization]
		public ActionResult Approve(int? id) {
			if (!id.HasValue)
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

			var userCourse = db.UserCourses.Include("Course").FirstOrDefault(uc => uc.Id == id);
			if (userCourse == null)
				return HttpNotFound();

			else if (!isCourseCreator(userCourse.Course))
				return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);

			userCourse.DateJoin = DateTime.Now;

			db.Entry(userCourse).State = EntityState.Modified;
			db.SaveChanges();

			return RedirectToAction("Members", "Course", new { id = userCourse.CourseId });
		}

		[PleAuthorization]
		[HttpPost]
		public ActionResult Approve(List<int> list) {
			if (!list.Any())
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

			foreach (var i in list) {
				var userCourse = db.UserCourses.Include("Course").FirstOrDefault(uc => uc.Id == i);
				if (userCourse == null)
					return HttpNotFound();

				else if (!isCourseCreator(userCourse.Course))
					return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);

				userCourse.DateJoin = DateTime.Now;

				db.Entry(userCourse).State = EntityState.Modified;
			}

			db.SaveChanges();

			return new HttpStatusCodeResult(HttpStatusCode.OK);
		}

		[PleAuthorization]
		public ActionResult Active(Guid? courseId) {
			var course = db.Courses.SingleOrDefault(i => i.Id == courseId);

			if (course == null)
				return HttpNotFound();

			if (course.IsCourseActive == true) {
				course.IsCourseActive = false;
			} else {
				course.IsCourseActive = true;
			}

			db.Entry(course).State = EntityState.Modified;
			db.SaveChanges();
			return RedirectToAction("Index", "Course", new { id = course.Id });
		}
		#endregion

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
			var identity = User.GetPrincipal()?.Identity as PleClaimsIdentity;
			if (identity == null)
				return false;
			return identity.HasClaim(PleClaimType.Member, courseId.ToString());
		}

		private bool isMember(Course course) {
			return isMember(course.Id);
		}

		private bool isWaiting(Guid? courseId) {
			if (courseId == null)
				return false;
			var identity = User.GetPrincipal()?.Identity as PleClaimsIdentity;
			if (identity == null)
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