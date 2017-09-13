using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PLE444.Models
{
	public class Event
	{
		public Event()
		{
			ID = Guid.NewGuid();
		}

		[Required]
		public Guid ID { get; set; }
		public String Name { get; set; }
		public DateTime DateTime { get; set; }
		public String Place { get; set; }
		public String Description { get; set; }
	}
}