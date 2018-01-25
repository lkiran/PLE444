using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace PLE.Service.Models
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
		public virtual IList<Tag> Tags { get; set; }
	}
}