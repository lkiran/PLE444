using System.Collections.Generic;

namespace PLE.Contract.DTOs.Responses
{
	public class RegisterUserResponseDto
	{
		public List<string> Errors { get; set; }

		public bool Status { get; set; }

		public string UserId { get; set; }
	}
}
