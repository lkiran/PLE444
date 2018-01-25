using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PLE444.Models
{
	public class UserCommunity
	{
		public UserCommunity()
		{
			Id = Guid.NewGuid();
			DateJoined = null;
			IsActive = true;
		}

		[Key]
		public Guid Id { get; set; }

		[ForeignKey("User")]
		public string UserId { get; set; }

		public bool IsActive { get; set; }

		public ApplicationUser User { get; set; }

		[ForeignKey("Community")]
		public Guid CommunityId { get; set; }

		public Community Community { get; set; }

		public DateTime? DateJoined { get; set; }
	}
}