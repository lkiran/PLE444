using PLE444.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using PLE.Contract.DTOs;
using PLE.Website.Service;
using PLE444.Models.Layout;

namespace PLE444.Controllers
{
	public class LayoutController : Controller
	{
		private PleDbContext db = new PleDbContext();
		private CourseService _courseService;
        private CommunityService _communityService;
        protected override void OnActionExecuting(ActionExecutingContext context) {
			_courseService = new CourseService(User.GetPrincipal()?.User?.Token.access_token);
            _communityService = new CommunityService(User.GetPrincipal()?.User?.Token.access_token);
        }

		[ChildActionOnly]
		public ActionResult Courses() {
			var userId = User.GetPrincipal()?.User?.Id;

			if (userId.IsNullOrWhiteSpace())
				return PartialView(new SidebarList<CourseDto>());

			var data = _courseService.GetCourseListByUser();
			var  model= new SidebarList<CourseDto>
			{
				Items = data,
				ActiveUserId = userId
			};
			return PartialView(model);
		}

		[ChildActionOnly]
		public ActionResult Communities() {
			var userId = User.GetPrincipal()?.User?.Id;


			if (userId.IsNullOrWhiteSpace())
				return PartialView(new List<Community>());

           var data= _communityService.GetCommunityListByUser();
            var model = new SidebarList<CommunityDto>
            {
                Items = data,
                ActiveUserId = userId
            };
            return PartialView(model);
		}

		public ActionResult LogedInUser() {
			var user = User.GetPrincipal()?.User;
			return PartialView(user);
		}

		[ChildActionOnly]
		public ActionResult Spaces() {
			var s = db.Spaces.ToList();
			return PartialView(s);
		}
	}
}