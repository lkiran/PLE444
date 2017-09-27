using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using PLE.Contract.DTOs;
using PLE.Website.Service.Core;

namespace PLE.Website.Service
{
	public class CourseService:IBaseService
	{
		public string Token { get; set; }

		private readonly PleClient _client;

		public CourseService(string token) {
			_client = new PleClient(token);
		}

		public List<CourseDto> GetCourseListByUser([Optional]string userId)
		{
			var result = _client.Get<List<CourseDto>>($"api/course/ListByUser/{userId}");
			if(result ==null)
				result=new List<CourseDto>();
			return result;
		}
	}
}
