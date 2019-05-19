using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace PLE.Service.Models
{
	public class Question
	{
		public Question() {
			AnswerOptions = new List<Answer>();
			UserAnswers = new List<UserAnswer>();
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

		[Key]
		public Guid Id { get; set; }

		public bool IsDeleted { get; set; }

		public string Title { get; set; }

		[AllowHtml]
		public string Description { get; set; }

		public AnswerType Answering { get; set; }

		public EvaluationType Evaluation { get; set; }

		public ICollection<Answer> AnswerOptions { get; set; }

		public virtual ICollection<UserAnswer> UserAnswers { get; set; }
	}
}