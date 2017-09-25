using System;

namespace PLE.Contract.DTOs
{
	public class DocumentDto
	{
		public int Id { get; set; }

		public UserDto Owner { get; set; }

		public string FilePath { get; set; }

		public string Description { get; set; }

		public DateTime DateUpload { get; set; }
	}
}
