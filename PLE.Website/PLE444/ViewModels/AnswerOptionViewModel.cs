using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace PLE444.ViewModels
{
	public class AnswerOptionViewModel
	{
		public Guid Id { get; set; }
		
		public Guid QuestionId { get; set; }
		
		[AllowHtml]
		[DisplayName("İçerik")]
		[Required(ErrorMessage = "İçerik zorunlu alandır")]
		public string Content { get; set; }
	}
}