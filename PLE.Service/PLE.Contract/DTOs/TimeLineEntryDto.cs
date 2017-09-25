using System;

namespace PLE.Contract.DTOs
{
	public class TimeLineEntryDto
	{
		public Guid Id { get; set; }

		public UserDto Creator { get; set; }
		
		public DateTime DateCreated { get; set; }

		public string Heading { get; set; }

		public string Content { get; set; }

		public string ColorClass { get; set; }

		public string IconClass { get; set; }
	}
}
