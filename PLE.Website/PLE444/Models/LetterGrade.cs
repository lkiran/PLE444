using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PLE444.Models
{
	public class LetterGrade
	{
		public LetterGrade() {
			From = 0;
			To = 100;
			IsActive = true;
		}

		public Guid Id { get; set; }

		public bool IsActive { get; set; }

		[Required]
		[ForeignKey("Course")]
		public Guid CourseId { get; set; }

		public virtual Course Course { get; set; }

		[Required(ErrorMessage = "İsim alanı boş bırakılamaz.")]
		[DisplayName("İsim")]
		public string Name { get; set; }

		[DisplayName("Açıklama")]
		public string Description { get; set; }

		[Required]
		[DisplayName("En Az")]
		public float From { get; set; }

		[Required]
		[DisplayName("En Yüksek")]
		public float To { get; set; }
	}
}