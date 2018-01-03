using System;
using System.Web.Mvc;

namespace PLE444.ViewModels
{
	public class NewMessageViewModel
	{
		public Guid ReplyId { get; set; }

		public Guid DiscussionId { get; set; }

		public string CurrentUserId { get; set; }

		public Guid CommunityId { get; set; }

		public string SenderId { get; set; }

		[AllowHtml]
		public string Content { get; set; }
	}
}