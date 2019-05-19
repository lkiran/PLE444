using System;

namespace PLE.Contract.DTOs
{
	public class QuizListDto
	{
		public Guid Id { get; set; }
		
		public bool IsPublished { get; set; }

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

		public bool CanAnswer => DateTime.Now >= AvailableOn && DateTime.Now < AvailableTill && IsPublished;
	}
}