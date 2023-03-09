using System;

namespace PLE.Contract.DTOs.Requests
{
	public class DuplicateCourseRequestDto
	{
		public Guid Id { get; set; }

		public string NewCode { get; set; }

		public string NewName { get; set; }
	}
}