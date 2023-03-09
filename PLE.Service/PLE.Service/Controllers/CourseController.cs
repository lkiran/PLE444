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
		#region Fields
		private readonly CourseService _courseService;
		#endregion
		
		#region Ctor
		public CourseController() {
			_courseService = new CourseService();
		}
		#endregion

		[HttpGet]
		[Route("Detail/{id}")]
		public IHttpActionResult Detail(Guid id) {
			try {
				var result = _courseService.Detail(id);
				return Ok(result);
			} catch (Exception e) {
				return InternalServerError(e);
			}
		}

		[HttpGet]
		[Route("ListByUser/{userId?}")]
		public IHttpActionResult ListByUser(string userId = null) {
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
		
		[HttpPost]
		[Authorize]
		[Route("Create")]
		public IHttpActionResult Create(CourseDto request) {
			try {
				request.CreatorId = User.Identity.GetUserId();
				var result = _courseService.Create(request);
				return Ok(result);
			} catch (Exception e) {
				return InternalServerError(e);
			}
		}

		[HttpGet]
		[Authorize]
		[Route("Ban/{id}")]
		public IHttpActionResult Ban(Guid id) {
			try {
				var result = _courseService.Ban(id);
				return Ok(result);
			} catch (Exception e) {
				return InternalServerError(e);
			}
		}

		[HttpGet]
		[Authorize]
		[Route("RemoveBan/{id}")]
		public IHttpActionResult RemoveBan(Guid id) {
			try {
				var result = _courseService.RemoveBan(id);
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

		#region Membership
		[HttpGet]
		[Route("Join/{courseId}")]
		public IHttpActionResult Join(Guid courseId) {
			try {
				if (User.Identity.GetUserId() == null)
					return Unauthorized();
				var userId = User.Identity.GetUserId();
				var result = _courseService.Join(userId, courseId);
				return Ok(result);
			} catch (Exception e) {
				return InternalServerError(e);
			}
		}

		[HttpGet]
		[Route("Leave/{courseId}")]
		public IHttpActionResult Leave(Guid courseId) {
			try {
				if (User.Identity.GetUserId() == null)
					return Unauthorized();
				var userId = User.Identity.GetUserId();
				var result = _courseService.Leave(userId, courseId);
				return Ok(result);
			} catch (Exception e) {
				return InternalServerError(e);
			}
		}

		[HttpGet]
		[Route("Approve/{ids}")]
		public IHttpActionResult Approve(string ids) {
			try {
				if (User.Identity.GetUserId() == null)
					return Unauthorized();
				var userId = User.Identity.GetUserId();
				var membershipIds = ids.Split(',').Select(int.Parse).ToList();
				var result = _courseService.Approve(membershipIds);
				return Ok(result);
			} catch (Exception e) {
				return InternalServerError(e);
			}
		}

		[HttpGet]
		[Route("Eject")]
		public IHttpActionResult Approve(string user, Guid @from) {
			try {
				if (User.Identity.GetUserId() == null)
					return Unauthorized();
				var result = _courseService.Eject(user, @from);
				return Ok(result);
			} catch (Exception e) {
				return InternalServerError(e);
			}
		} 
		#endregion
	}
}