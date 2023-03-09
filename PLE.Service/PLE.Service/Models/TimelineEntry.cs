using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace PLE.Service.Models
{
	public class TimelineEntry
	{
		public static class Icon
		{
			public static string Plus = "ti ti-plus";
		}

		public static class Color
		{
			public static string Primary = "timeline-primary";
		}

		public TimelineEntry() {
			Id = Guid.NewGuid();
			ColorClass = Color.Primary;
			IconClass = Icon.Plus;
		}

		public Guid Id { get; set; }

		public ApplicationUser Creator { get; set; }

		[ForeignKey("Creator")]
		public string CreatorId { get; set; }

		public DateTime DateCreated { get; set; }

		public string Heading { get; set; }

		[AllowHtml]
		public string Content { get; set; }

		public string ColorClass { get; set; }

		public string IconClass { get; set; }
	}
}