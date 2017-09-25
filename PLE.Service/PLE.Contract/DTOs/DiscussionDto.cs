using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLE.Contract.DTOs
{
	public class DiscussionDto
	{
		public Guid Id { get; set; }

		public string Topic { get; set; }

		public UserDto Creator { get; set; }

		public DateTime DateCreated { get; set; }

		public virtual ICollection<MessageDto> Messages { get; set; }

		public virtual ICollection<ReadingDto> Readings { get; set; }
	}
}
