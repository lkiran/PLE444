using System;
using System.ComponentModel.DataAnnotations;

namespace PLE.Service.Models
{
	public class Interest
	{
		public Interest()
		{
			Id = Guid.NewGuid();
		}

		[Required]
		public Guid Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
	}
}