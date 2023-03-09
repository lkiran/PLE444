using System.Collections.Generic;
using PLE.Contract.DTOs;

namespace PLE444.ViewModels
{
	public class QuizDetailViewModel
	{
		public QuizDto Quiz { get; set; }
	
		public List<UserAnswerDto> Answers { get; set; }
	}
}