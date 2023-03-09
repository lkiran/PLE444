using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PLE444.Models
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

		
		[Required(ErrorMessage = "Ödev başlığı girilmesi zorunludur")]
		[DisplayName("Başlık")]
		public string Title { get; set; }

		[Required]
		[AllowHtml]
		[DisplayName("İçerik")]
		public string Description { get; set; }

		[Required]
		[DisplayName("Teslim Tarihi")]
		[DataType(DataType.Date)]
		[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}", ApplyFormatInEditMode = true)]
		public DateTime Deadline { get; set; }

		public DateTime DateAdded { get; set; }
        public bool IsHidden { get; set; }

        public virtual ICollection<Document> Uploads { get; set; }

		public bool IsFeedbackPublished { get; set; }
       
    }
}