using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace PLE.Service.Models
{
	public class Message
	{
		public Message() {
			ID = Guid.NewGuid();
		}
		public Guid ID { get; set; }

		[AllowHtml]
		public string Content { get; set; }

		[ForeignKey("Sender")]
		public string SenderId { get; set; }

		public ApplicationUser Sender { get; set; }

		public DateTime DateSent { get; set; }

		public virtual ICollection<Message> Replies { get; set; }
	}
}