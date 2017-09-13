using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PLE.Contract.DTOs
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

		public User User { get; set; }

		[ForeignKey("Community")]
		public Guid CommunityId { get; set; }

		public Community Community { get; set; }

		public DateTime? DateJoined { get; set; }
	}
}