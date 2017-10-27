using System.Collections.Generic;
using System.Runtime.InteropServices;
using PLE.Contract.DTOs;
using PLE.Website.Service.Core;

namespace PLE.Website.Service
{
	public class CourseService
	{
		private readonly PleClient _client;

		public CourseService() {
			_client = new PleClient();
		}

		public List<CourseDto> GetCourseListByUser([Optional]string userId) {
			var result = _client.Get<List<CourseDto>>($"api/course/ListByUser/{userId}");

			if (result == null)
				result = new List<CourseDto>();

			return result;
		}
	}
}
