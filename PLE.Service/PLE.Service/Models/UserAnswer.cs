using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PLE.Service.Models
{
	public class UserAnswer
	{
		[Key]
		public Guid Id { get; set; }

		[ForeignKey("User")]
		public string UserId { get; set; }

		public ApplicationUser User { get; set; }

		public DateTime AnsweredOn { get; set; }
		
		[ForeignKey("Answer")]
		public Guid AnswerId { get; set; }

		public Answer Answer { get; set; }
	}
}