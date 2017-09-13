using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PLE.Contract.DTOs
{
	public class Community
	{
		public Community()
		{
			Id = Guid.NewGuid();
			IsActive = true;
		}

		[Key]
		public Guid Id { get;  set; }

		public string Name { get; set; }
		
		public string Description { get; set; }

		public DateTime DateCreated { get; set; }

		public bool IsActive { get; set; }

		public bool IsOpen { get; set; }

		public bool IsHiden { get; set; }

		[ForeignKey("Owner")]
		public string OwnerId { get; set; }
		
		public User Owner { get; set; }

		[ForeignKey("Space")]
		public int SpaceId { get; set; }

		public Space Space { get; set; }

		public virtual ICollection<Discussion> Discussions { get; set; }
	}
}