using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace PLE.Service.Models
{
	public class Chapter
	{
		public Chapter() {
			Id = Guid.NewGuid();
			IsActive = true;
		}

		[Required]
		public Guid Id { get; set; }

		[HiddenInput]
		public Guid CourseId { get; set; }

		[ForeignKey("CourseId")]
		public virtual Course Course { get; set; }

		[Required]
		public string Title { get; set; }

		[Required]
		[AllowHtml]
		public string Description { get; set; }

		public DateTime DateAdded { get; set; }

		public bool IsActive { get; set; }

		public virtual ICollection<Material> Materials { get; set; }
	}
}