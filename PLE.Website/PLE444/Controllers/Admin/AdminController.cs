using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using Microsoft.Owin.Security;
using PLE.Contract.DTOs;
using PLE.Contract.Enums;
using PLE.Website.Service;
using PLE444.Models;
using PLE444.ViewModels;

namespace PLE444.Controllers.Admin
{
	[PleAdminAuthorization]
	public class AdminController : Controller
	{
		#region Fields
		private readonly PleDbContext _db;
		private readonly CourseService _courseService;
		#endregion

		#region Ctor
		public AdminController() {
			_db = new PleDbContext();
			_courseService = new CourseService();
		}
		#endregion

		// GET: Admin
		public ActionResult Index() {
			var model = new AdminIndexViewModel {
				CourseCount = _db.Courses.Count(),
				GroupCount = _db.Communities.Count(),
				UserCount = _db.Users.Count()
			};
			return View(model);
		}

		// GET: Admin/Users
		public ActionResult Users() {
			var users = _db.Users.OrderByDescending(u => u.Role == RoleType.Admin);
			return View(users.ToList());
		}

		// GET: Admin/Courses
		public ActionResult Courses() {
			var courses = _db.Courses
				.OrderBy(c => c.IsBanned)
				.ThenByDescending(c => c.DateCreated);
			return View(courses.ToList());
		}

		// GET: Admin/Groups
		public ActionResult Groups() {
			var groups = _db.Communities.OrderByDescending(c => c.DateCreated);
			return View(groups.ToList());
		}

		// GET: Admin/ViewCourse
		public ActionResult ViewCourse(Guid id) {
			#region Add claim
			var claims = HttpContext.Session["Claims"] as List<Claim> ?? new List<Claim>();
			var user = HttpContext.Session["User"] as UserDto;

			if (user != null) {
				if (!claims.Any()) {
					claims.Add(new Claim(PleClaimType.Creator, id.ToString()));
				}
			}

			var identity = new PleClaimsIdentity(claims,"PLE");
			HttpContext.User = new UserPrincipal(identity, user);
			#endregion
	
			return RedirectToAction("Index", "Course", new { id });
		}

		// GET: Admin/BanCourse
		public ActionResult BanCourse(Guid id) {
			_courseService.Ban(id);

			return RedirectToAction("Courses", "Admin");
		}

		// GET: Admin/RemoveBanCourse
		public ActionResult RemoveBanCourse(Guid id) {
			_courseService.RemoveBan(id);

			return RedirectToAction("Courses", "Admin");
		}
	}
}