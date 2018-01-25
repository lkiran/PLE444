using System;
using System.ComponentModel.DataAnnotations;

namespace PLE.Service.Models
{
	public class Friendship
	{
		public Friendship()
		{
			Id = Guid.NewGuid();
		}

		[Required]
		public Guid Id { get; set; }
		public string userID { get; set; }

		public string FriendID { get; set; }

		public bool isApproved { get; set; }
	}
}