using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PLE.Contract.DTOs
{
	public class PrivateMessage
	{
		[Key]
		public int Id { get; set; }

		[ForeignKey("Sender")]
		public string SenderId { get; set; }

		public User Sender { get; set; }

		[ForeignKey("Receiver")]
		public string ReceiverId { get; set; }

		public User Receiver { get; set; }

		public string Content { get; set; }

		public DateTime DateSent { get; set; }

		public bool IsRead { get; set; }
	}
}