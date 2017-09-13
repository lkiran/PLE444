using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace PLE.Contract.DTOs
{
	public class Message
	{
		public Message()
		{
			Id = Guid.NewGuid();
		}
		public Guid Id { get; set; }

		public string Content { get; set; }

		[ForeignKey("Sender")]
		public string SenderId { get; set; }

		public User Sender { get; set; }

		public DateTime DateSent { get; set; }
	}
}