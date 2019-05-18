using System;
using System.Collections.Generic;

namespace PLE.Contract.DTOs
{
	public class CourseDto
	{
		public Guid Id { get; set; }

		public Guid? CopiedFromId { get; set; }

		public CourseDto CopiedFrom { get; set; }

		public string CreatorId { get; set; }

		public UserDto Creator { get; set; }

		public string Code { get; set; }

		public string Name { get; set; }

		public string Description { get; set; }

		public DateTime DateCreated { get; set; }

		public bool CanEveryoneJoin { get; set; }

		public bool IsCourseActive { get; set; }

		public bool IsBanned { get; set; }
		
		public virtual ICollection<ChapterDto> Chapters { get; set; }

		public virtual ICollection<AssignmentDto> Assignments { get; set; }

		public virtual ICollection<DiscussionDto> Discussion { get; set; }

		public string Heading => $"{Code} - {Name}";
	}
}