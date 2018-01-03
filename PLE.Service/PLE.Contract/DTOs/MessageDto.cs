using System;
using System.Collections.Generic;

namespace PLE.Contract.DTOs
{
	public class MessageDto
	{
		public Guid Id { get; set; }
		
		public string Content { get; set; }
		
		public UserDto Sender { get; set; }

		public DateTime DateSent { get; set; }

		public virtual ICollection<MessageDto> Replies { get; set; }
	}
}
