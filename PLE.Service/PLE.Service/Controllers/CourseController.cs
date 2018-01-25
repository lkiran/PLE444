using System.Web.Http;
using Microsoft.AspNet.Identity;
using PLE.Service.Implementations;

namespace PLE.Service.Controllers
{
	[RoutePrefix("Api/Course")]
	public class CourseController : BaseApiController
	{
		private CourseService _courseService;

		public CourseController() {
			_courseService = new CourseService();
		}

		[HttpGet]
		[Route("ListByUser/{userId?}")]
		public IHttpActionResult GetByUser(string userId = null) {
			if (string.IsNullOrWhiteSpace(userId)) {
				if (User.Identity.GetUserId() == null)
					return Unauthorized();
				userId = User.Identity.GetUserId();
			}

			var result = _courseService.GetCourseListByUser(userId);

			return Ok(result);
		}
	}
}
