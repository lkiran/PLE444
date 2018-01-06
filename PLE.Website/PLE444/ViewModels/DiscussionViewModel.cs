using System;
using PLE444.Models;
using System.Collections.Generic;

namespace PLE444.ViewModels
{
	public class DiscussionViewModel
	{
		public Guid CId{ get; set; }

		public string CurrentUserId{ get; set; }

		public string Role{ get; set; }

		public List<Discussion> Discussion { get; set; }
	}
}