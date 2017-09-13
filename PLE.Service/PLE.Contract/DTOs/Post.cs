using System;
using System.ComponentModel.DataAnnotations;

namespace PLE.Contract.DTOs
{
	public class Post
	{
		public Post()
		{
			Id = Guid.NewGuid();
		}

		[Required]
		public Guid Id { get; set; }

		public string UserId { get; set; }

		public string Content { get; set; }
	}
}