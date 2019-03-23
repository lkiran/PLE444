using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PLE.Service.Models
{
	public class Answer
	{
		[Key]
		public Guid Id { get; set; }

		[ForeignKey("User")]
		public string UserId { get; set; }

		public ApplicationUser User { get; set; }

		public DateTime AnsweredOn { get; set; }

		public Dictionary<int,string> Answers { get; set; }
	}
}