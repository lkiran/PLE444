using System;
using System.Web.Mvc;
using PLE444.Models;
using PLE.Contract.Enums;
using PLE.Website.Service;

namespace PLE444.Controllers
{
	[PleAuthorization]
	public class QuizController : Controller
	{
		#region Fields
		private PleDbContext db = new PleDbContext();
		private CourseService _courseService;
		private EmailService ms = new EmailService();
		#endregion

		#region Ctor
		public QuizController() {
			_courseService = new CourseService();
		}
		#endregion

		public ActionResult Index(int courseId) {

			return View();
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