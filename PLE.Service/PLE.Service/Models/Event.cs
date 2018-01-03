using System;
using System.ComponentModel.DataAnnotations;

namespace PLE.Service.Models
{
	public class Event
	{
		public Event() {
			ID = Guid.NewGuid();
		}

		[Required]
		public Guid ID { get; set; }

		public string Name { get; set; }

		public DateTime DateTime { get; set; }

		public string Place { get; set; }

		public string Description { get; set; }
	}
}