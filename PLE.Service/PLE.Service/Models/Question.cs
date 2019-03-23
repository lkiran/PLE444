using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PLE.Service.Models
{
	public class Question
	{
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

		[Key]
		public Guid Id { get; set; }

		public string Title { get; set; }

		public string Description { get; set; }

		public AnswerType Answering { get; set; }

		public EvaluationType Evaluation { get; set; }

		public Dictionary<int, string> AnswerOptions { get; set; }

		public Dictionary<int, string> CorrectOptions { get; set; }
	}
}