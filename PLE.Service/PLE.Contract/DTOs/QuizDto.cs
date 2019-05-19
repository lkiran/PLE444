using System;
using System.Collections.Generic;

namespace PLE.Contract.DTOs
{
	public class QuizDto
	{
		public QuizDto() {
			Questions = new List<QuestionDto>();
			AvailableTill = DateTime.MinValue;
		}

		public Guid Id { get; set; }

		public string Name { get; set; }

		public string Description { get; set; }

		public DateTime AvailableOn { get; set; }

		private DateTime _availableTill;
		public DateTime AvailableTill {
			get => _availableTill == DateTime.MinValue
				? AvailableOn + TimeSpan
				: _availableTill;

			set => _availableTill = value;
		}

		public TimeSpan TimeSpan { get; set; }

		public Guid CourseId { get; set; }

		public CourseDto Course { get; set; }

		public List<QuestionDto> Questions { get; set; }

		public bool CanAnswer => DateTime.Now < AvailableTill;
	}
}