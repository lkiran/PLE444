using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PLE444.Models
{
	public class Post
	{
		public Post()
		{
			Id = Guid.NewGuid();
		}

		[Required]
		public Guid Id { get; private set; }
		public String UserId { get; set; }
		public String Content { get; set; }
		public virtual IList<Tag> Tags { get; set; }
	}
}