using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using PLE.Contract.Enums;
using PLE444.Models;
using PLE444.ViewModels;

namespace PLE444.Controllers.Admin
{
	[PleAdminAuthorization]
	public class AdminController : Controller
	{
		#region Fields
		private readonly PleDbContext _db;
		#endregion

		#region Ctor
		public AdminController() {
			_db = new PleDbContext();
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
			var courses = _db.Courses.OrderByDescending(c => c.DateCreated);
			return View(courses.ToList());
		}

		// GET: Admin/Groups
		public ActionResult Groups() {
			var groups = _db.Communities.OrderByDescending(c => c.DateCreated);
			return View(groups.ToList());
		}
	}
}