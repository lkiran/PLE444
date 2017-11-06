using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PLE444.Models;

namespace PLE444.ViewModels
{
	public class DiscussionViewModel
	{
		public Guid CourseId{ get; set; }

		public string CurrentUserId{ get; set; }

		public string Role{ get; set; }

		public List<Discussion> Discussion { get; set; }
	}
}