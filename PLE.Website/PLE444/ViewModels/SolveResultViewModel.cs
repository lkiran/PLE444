using System;
using PLE.Contract.DTOs;

namespace PLE444.ViewModels
{
	public class SolveResultViewModel
	{
		public QuestionDto.AnswerType AnswerType { get; set; }

		public Guid QuestionId { get; set; }

		public string Value { get; set; }

		public bool IsValid {
			get {
				switch (AnswerType) {
					case QuestionDto.AnswerType.ShortAnswer:
						return Value != "<p><br></p>";
					case QuestionDto.AnswerType.SingleSelection:
						Guid g;
						return Guid.TryParse(Value, out g);
					default:
						return false;
				}
			}
		}
	}
}