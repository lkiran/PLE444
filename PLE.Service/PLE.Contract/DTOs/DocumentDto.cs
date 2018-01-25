using System;

namespace PLE.Contract.DTOs
{
	public class DocumentDto
	{
		public string OwnerId { get; set; }

		public string FilePath { get; set; }

		public string Description { get; set; }

		public DateTime DateUpload { get; set; }

		public string Feedback { get; set; }
	}
}
