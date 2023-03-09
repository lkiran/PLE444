using System;

namespace PLE.Contract.DTOs
{
	public class TimelineEntryDto
	{
		public static class Icon
		{
			public static string Plus = "ti ti-plus";
		}

		public static class Color
		{
			public static string Primary = "timeline-primary";
		}

		public Guid Id { get; set; }

		public UserDto Creator { get; set; }
		
		public DateTime DateCreated { get; set; }

		public string Heading { get; set; }

		public string Content { get; set; }

		public string ColorClass { get; set; }

		public string IconClass { get; set; }
	}
}
