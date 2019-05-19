using System;

namespace PLE.Contract.DTOs
{
	public class UserAnswerDto
	{
		public Guid Id { get; set; }
		
		public string UserId { get; set; }

		public UserDto User { get; set; }

		public DateTime AnsweredOn { get; set; }
		
		public Guid AnswerId { get; set; }

		public AnswerDto Answer { get; set; }
	}
}