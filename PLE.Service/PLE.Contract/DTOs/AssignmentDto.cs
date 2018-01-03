using System;
using System.Collections.Generic;

namespace PLE.Contract.DTOs
{
	public class AssignmentDto
	{

		public Guid Id { get; set; }

		public bool IsActive { get; set; }

		public CourseDto Course { get; set; }

		public Guid CourseId { get; set; }

		public string Title { get; set; }

		public string Description { get; set; }

		public DateTime Deadline { get; set; }

		public DateTime DateAdded { get; set; }

		public virtual ICollection<DocumentDto> Uploads { get; set; }

		public bool IsFeedbackPublished { get; set; }
	}
}