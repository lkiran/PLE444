using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLE.Contract.DTOs
{
	public class MessageDto
	{
		public Guid Id { get; set; }
		
		public string Content { get; set; }
		
		public UserDto Sender { get; set; }

		public DateTime DateSent { get; set; }
	}
}
