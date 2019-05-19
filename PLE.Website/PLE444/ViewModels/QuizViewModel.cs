using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using PLE.Contract.DTOs;

namespace PLE444.ViewModels
{
	public class QuizViewModel
	{
		public QuizViewModel() {
			Questions = new List<QuestionDto>();
		}

		public Guid Id { get; set; }

		[DisplayName("Durumu")]
		public bool IsPublished { get; set; }

		public Guid CourseId { get; set; }

		[Required(ErrorMessage = "Test başlığı zorunlu alandır")]
		[DisplayName("Başlık")]
		public string Name { get; set; }

		[AllowHtml]
		[Required(ErrorMessage = "Açıklama zorunlu alandır")]
		[DisplayName("Açıklama")]
		public string Description { get; set; }

		[DisplayName("Başlangıç Tarihi")]
		public DateTime AvailableOn { get; set; }

		[DisplayName("Bitiş Tarihi")]
		public DateTime AvailableTill { get; set; }

		public List<QuestionDto> Questions { get; set; }
	}
}