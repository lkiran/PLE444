using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PLE.Service.Models
{
	public class Quiz
	{
		public Quiz()
		{
			Questions = new List<Question>();
		}

		[Key]
		public Guid Id { get; set; }

		public string Name { get; set; }

		public string Description { get; set; }

		public DateTime AvailableOn { get; set; }

		public DateTime AvailableTill { get; set; }

		public TimeSpan TimeSpan { get; set; }

		public bool IsDeleted { get; set; }

		[ForeignKey("Course")]
		public Guid CourseId { get; set; }

		public Course Course { get; set; }

		public virtual ICollection<Question> Questions { get; set; }
	}
}