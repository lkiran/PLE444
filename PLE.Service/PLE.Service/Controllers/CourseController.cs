using System;
using System.Collections.Generic;
using System.IdentityModel.Claims;
using System.Linq;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using PLE.Contract.DTOs;
using PLE.Contract.DTOs.Requests;
using PLE.Service.Implementations;

namespace PLE.Service.Controllers
{
	[RoutePrefix("Api/Course")]
	public class CourseController : BaseApiController
	{
		private readonly CourseService _courseService;

		public CourseController() {
			_courseService = new CourseService();
		}

		[HttpPost]
		[Authorize]
		[Route("Create")]
		public IHttpActionResult Create(CourseDto request) {
			try
			{
				request.CreatorId = User.Identity.GetUserId();
				var result = _courseService.Create(request);
				return Ok(result);
			} catch (Exception e) {
				return InternalServerError(e);
			}
		}

		[HttpPost]
		[Authorize]
		[Route("Duplicate")]
		public IHttpActionResult Duplicate(DuplicateCourseRequestDto request) {
			try {
				var result = _courseService.Duplicate(request);
				return Ok(result);
			} catch (Exception e) {
				return InternalServerError(e);
			}
		}

		[HttpGet]
		[Route("ListByUser/{userId?}")]
		public IHttpActionResult GetByUser(string userId = null) {
			try {
				if (string.IsNullOrWhiteSpace(userId)) {
					if (User.Identity.GetUserId() == null)
						return Unauthorized();
					userId = User.Identity.GetUserId();
				}
				var result = _courseService.GetCourseListByUser(userId);
				return Ok(result);
			} catch (Exception e) {
				return InternalServerError(e);
			}
		}

		[HttpGet]
		[Authorize]
		[Route("GetClaims")]
		public IHttpActionResult GetAllClaims() {
			try {
				var userId = User.Identity.GetUserId();
				var result = _courseService.GetAllClaims(userId);
				return Ok(result);
			} catch (Exception e) {
				return InternalServerError(e);
			}
		}

		[HttpGet]
		[Route("Detail/{id}")]
		public IHttpActionResult GetByUser(Guid id) {
			try {
				var result = _courseService.Detail(id);
				return Ok(result);
			} catch (Exception e) {
				return InternalServerError(e);
			}
		}
	}
}