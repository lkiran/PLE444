using System.Collections.Generic;

namespace PLE.Contract.DTOs.Responses
{
	public class ResponseDto
	{
		public List<string> Errors { get; set; }

		public bool Status { get; set; }
	}
}