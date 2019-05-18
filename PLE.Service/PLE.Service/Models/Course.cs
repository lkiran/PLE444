using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace PLE.Service.Models
{
	public class Course
	{
		public Course() {
			Id = Guid.NewGuid();
			CanEveryoneJoin = false;
			IsCourseActive = true;
		}

		[Required]
		public Guid Id { get; set; }

		public Guid? CopiedFromId { get; set; }

		[ForeignKey("Creator")]
		public string CreatorId { get; set; }

		public ApplicationUser Creator { get; set; }

		public string Code { get; set; }

		public string Name { get; set; }

		[AllowHtml]
		public string Description { get; set; }

		public DateTime DateCreated { get; set; }

		public bool CanEveryoneJoin { get; set; }

		public bool IsCourseActive { get; set; }
		
		public bool IsBanned { get; set; }

		public virtual ICollection<Chapter> Chapters { get; set; }

		public virtual ICollection<Assignment> Assignments { get; set; }

		public virtual ICollection<Quiz> Quizes { get; set; }

		public virtual ICollection<Discussion> Discussion { get; set; }

		public virtual ICollection<TimelineEntry> Timeline { get; set; }

		public string Heading => $"{Code} - {Name}";
	}
}