using System.Collections.Generic;
using PLE444.Models;

namespace PLE444.ViewModels
{
	public class CourseGrades
	{
		public string CurrentUserId { get; set; }

		public Course CourseInfo { get; set; }

		public IEnumerable<UserCourse> CourseUsers { get; set; }

		public IEnumerable<LetterGrade> LetterGrades { get; set; }

		public IEnumerable<GradeType> GradeTypes { get; set; }

		public IEnumerable<UserGrade> UserGrades { get; set; }
	}
}