using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace PLE.Contract.DTOs
{
	public class TimelineEntry
	{
		public TimelineEntry()
		{
			Id = Guid.NewGuid();
			ColorClass = "timeline-primary";
			IconClass = "ti ti-plus";
		}

		public Guid Id { get; set; }

		public User Creator { get; set; }

		[ForeignKey("Creator")]
		public string CreatorId { get; set; }

		public DateTime DateCreated { get; set; }

		public string Heading { get; set; }

		public string Content { get; set; }

		public string ColorClass { get; set; }

		public string IconClass { get; set; }
	}
}