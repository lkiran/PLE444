using System;
using System.ComponentModel.DataAnnotations;

namespace PLE.Contract.DTOs
{
	public class Event
	{
		public Event()
		{
			Id = Guid.NewGuid();
		}

		[Required]
		public Guid Id { get; set; }
		public string Name { get; set; }
		public DateTime DateTime { get; set; }
		public string Place { get; set; }
		public string Description { get; set; }
	}
}