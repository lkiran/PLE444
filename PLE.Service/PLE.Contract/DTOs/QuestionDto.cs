using System;
using System.Collections.Generic;

namespace PLE.Contract.DTOs
{
	public class QuestionDto
	{
		public QuestionDto() {
			AnswerOptions = new List<AnswerDto>();
		}

		public enum AnswerType
		{
			NotSpecified = 0,
			SingleSelection,
			MultipleSelection,
			ShortAnswer
		}

		public enum EvaluationType
		{
			NotSpecified = 0,
			Matches,
			Contains,
			Manual
		}

		public Guid Id { get; set; }

		public string Title { get; set; }

		public string Description { get; set; }

		public AnswerType Answering { get; set; }

		public EvaluationType Evaluation { get; set; }
		
		public QuizDto Quiz { get; set; }

		public List<AnswerDto> AnswerOptions { get; set; }

	}
}