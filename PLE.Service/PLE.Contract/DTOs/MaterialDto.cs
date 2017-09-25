using System;
using System.Collections.Generic;

namespace PLE.Contract.DTOs
{
	public class MaterialDto
	{
		public Guid Id { get; set; }
		
		public UserDto Owner { get; set; }
		
		public string Title { get; set; }

		public string Description { get; set; }

		public DateTime DateAdded { get; set; }

		public virtual ICollection<DocumentDto> Documents { get; set; }
	}
}
