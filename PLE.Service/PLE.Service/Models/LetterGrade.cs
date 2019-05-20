using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PLE.Service.Models
{
	public class LetterGrade
	{
		public LetterGrade() {
			From = 0;
			To = 100;
			IsActive = true;
			Id = new Guid();
		}

		[Key]
		public Guid Id { get; set; }

		public bool IsActive { get; set; }

		[ForeignKey("Course")]
		public Guid CourseId { get; set; }

		public virtual Course Course { get; set; }

		public string Name { get; set; }

		public string Description { get; set; }

		public float From { get; set; }

		public float To { get; set; }
	}
}