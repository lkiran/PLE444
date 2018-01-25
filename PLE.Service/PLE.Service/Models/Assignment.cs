using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace PLE.Service.Models
{
	public class Assignment
	{
		public Assignment()
		{
			Id = Guid.NewGuid();
			IsActive = true;
			//IsFeedbackPublished = false;
		}

		[Key]
		public Guid Id { get; set; }

		public bool IsActive { get; set; }

		public Course Course { get; set; }

		[Required]
		[ForeignKey("Course")]
		public Guid CourseId { get; set; }

		[Required]
		[DisplayName("Başlık")]
		public string Title { get; set; }

		[Required]
		[AllowHtml]
		[DisplayName("İçerik")]
		public string Description { get; set; }

		[Required]
		[DisplayName("Teslim Tarihi")]
		[DataType(DataType.Date)]
		[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm:ss}", ApplyFormatInEditMode = true)]
		public DateTime Deadline { get; set; }

		public DateTime DateAdded { get; set; }

		public virtual ICollection<Document> Uploads { get; set; }

		public bool IsFeedbackPublished { get; set; }
	}
}