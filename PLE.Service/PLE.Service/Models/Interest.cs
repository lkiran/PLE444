using System;
using System.ComponentModel.DataAnnotations;

namespace PLE.Service.Models
{
	public class Interest
	{
		public Interest() {
			ID = Guid.NewGuid();
		}

		[Required]
		public Guid ID { get; set; }

		public string Name { get; set; }

		public string Description { get; set; }
	}
}