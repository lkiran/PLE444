using PLE444.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PLE444.ViewModels
{
	public class DiscussionMessages
	{
			public Guid CId { get; set; }

			public string CurrentUserId { get; set; }

			public string Role { get; set; }

			public Discussion Discussion { get; set; }
		    
		    public bool isActive { get; set; }
		}
}