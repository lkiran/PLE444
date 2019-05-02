using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Claims;
using PLE.Contract.DTOs;
using PLE.Contract.DTOs.Requests;
using PLE.Website.Service.Core;

namespace PLE.Website.Service
{
	public class CourseService
	{
		private readonly PleClient _client;

		public CourseService() {
			_client = new PleClient();
		}

		public bool Join(Guid courseId) {
			var result = _client.Get<bool>($"api/course/Join/{courseId}");
			return result;
		}

		public bool Leave(Guid courseId) {
			var result = _client.Get<bool>($"api/course/Leave/{courseId}");
			return result;
		}

		public bool Approve(List<int> ids) {
			var result = _client.Get<bool>($"api/course/Approve/{string.Join(",", ids)}");
			return result;
		}

		public bool Eject(string userId, Guid courseId) {
			var result = _client.Get<bool>($"api/course/Eject?user={userId}&from={courseId}");
			return result;
		}

		public Guid Create(CourseDto request) {
			var result = _client.Post<Guid>("api/course/Create", request);
			return result;
		}

		public Guid Duplicate(DuplicateCourseRequestDto request) {
			var result = _client.Post<Guid>("api/course/Duplicate", request);
			return result;
		}
		public List<CourseDto> GetCourseListByUser([Optional]string userId) {
			var result = _client.Get<List<CourseDto>>($"api/course/ListByUser/{userId}");
			if (result == null)
				result = new List<CourseDto>();

			return result;
		}

		public CourseDetailDto Detail(Guid id) {
			var result = _client.Get<CourseDetailDto>($"api/course/Detail/{id}");
			return result;
		}



		public List<Claim> GetClaims() {
			var result = _client.Get<List<ClaimDto>>("api/course/GetClaims");
			return result.Select(c => new Claim(c.Key, c.Value)).ToList();
		}
	}
}