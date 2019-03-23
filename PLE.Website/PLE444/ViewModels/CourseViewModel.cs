using System.Collections.Generic;
using PLE.Contract.DTOs;
using PLE444.Models;

namespace PLE444.ViewModels
{
	public class CourseViewModel
	{
		public CourseDetailDto Course { get; set; }

		public bool IsCourseCreator { get; set; }

		public bool IsMember { get; set; }

		public bool IsWaiting { get; set; }
	}

	public class CourseMembers
	{
		public List<UserCourse> Members { get; set; }

		public bool CanEdit { get; set; }

		public Course Course { get; set; }
	}
}