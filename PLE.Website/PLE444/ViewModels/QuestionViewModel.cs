using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using PLE.Contract.DTOs;

namespace PLE444.ViewModels
{
	public class QuestionViewModel
	{
		public QuestionViewModel()
		{
			AnswerOptions=new List<AnswerDto>();
		}

		public Guid Id { get; set; }
		
		public Guid QuizId { get; set; }
		
		[DisplayName("Başlık")]
		[Required(ErrorMessage = "Başlık zorunlu alandır")]
		public string Title { get; set; }

		[AllowHtml]
		[DisplayName("Açıklama")]
		[Required(ErrorMessage = "Açıklama zorunlu alandır")]
		public string Description { get; set; }

		[DisplayName("Cevaplama Türü")]
		[Required(ErrorMessage = "Cevaplama Türü zorunlu alandır")]
		public QuestionDto.AnswerType Answering { get; set; }
		
		public List<AnswerDto> AnswerOptions { get; set; }
	}
}