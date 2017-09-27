using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using PLE.Contract.DTOs;
using PLE.Service.Models;

namespace PLE.Service.Implementations
{
	public class CourseService
	{
		private readonly PleDbContext _db = new PleDbContext();

		public List<CourseDto> GetCourseListByUser(string userId) {
			var userCourses = _db.UserCourses.Where(uc => uc.UserId == userId && uc.IsActive);
			var courses = _db.Courses.Where(c => c.CreatorId == userId);
			var data = (from p in userCourses select p.Course).Union(courses);
			var result = Mapper.Map<List<CourseDto>>(data.ToList());

			return result;
		}
	}
}