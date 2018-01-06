using System;
using System.Web.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PLE.Service.Models
{
	public class Community
	{
		public Community() {
			Id = Guid.NewGuid();
			IsActive = true;
		}

		[Key]
		public Guid Id { get; set; }

		public string Name { get; set; }

		[AllowHtml]
		public string Description { get; set; }

		public DateTime DateCreated { get; set; }

		public bool IsActive { get; set; }

		public bool IsOpen { get; set; }

		public bool IsHiden { get; set; }

		[ForeignKey("Owner")]
		public string OwnerId { get; set; }

		public ApplicationUser Owner { get; set; }

		public virtual ICollection<Discussion> Discussions { get; set; }
	}
}