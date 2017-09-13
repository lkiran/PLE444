using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PLE.Contract.DTOs
{
	public class Material
	{
		public Material()
		{
			Id = Guid.NewGuid();
			IsActive = true;
		}

		[Key]
		public Guid Id { get; set; }

		public bool IsActive { get; set; }

		public User Owner { get; set; }

		[ForeignKey("Owner")]
		public string OwnerId { get; set; }

		[Required]
		public string Title { get; set; }

		[Required]
		public string Description { get; set; }

		public DateTime DateAdded { get; set; }

		public virtual ICollection<Document> Documents { get; set; }
	}
}