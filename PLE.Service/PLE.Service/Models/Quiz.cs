using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace PLE.Service.Models
{
	public class Quiz
	{
		public Quiz() {
			AvailableTill = DateTime.MinValue;
			Questions = new List<Question>();
		}

		[Key]
		public Guid Id { get; set; }

		public bool IsDeleted { get; set; }

		public bool IsPublished { get; set; }

		public string Name { get; set; }

		[AllowHtml]
		public string Description { get; set; }

		public DateTime AvailableOn { get; set; }

		private DateTime _availableTill;
		public DateTime AvailableTill {
			get => _availableTill == DateTime.MinValue
				? AvailableOn + TimeSpan
				: _availableTill;

			set => _availableTill = value;
		}

		public TimeSpan TimeSpan { get; set; }

		[ForeignKey("Course")]
		public Guid CourseId { get; set; }

		public Course Course { get; set; }

		public virtual ICollection<Question> Questions { get; set; }

		public bool CanAnswer => DateTime.Now >= AvailableOn && DateTime.Now < AvailableTill && IsPublished && !IsDeleted;
	}
}