using System;

namespace PLE.Contract.DTOs
{
	public class AnswerDto
	{
		public Guid Id { get; set; }

		public string Content { get; set; }
		
		public QuestionDto Question { get; set; }
	}
}