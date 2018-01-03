using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PLE444.Models
{
	public class SendMessageParametersVm
	{
		public int messageId { get; set; }

		[Required]
		public int discussionId { get; set; }
		[Required]
		public int courseId { get; set; }
		[AllowHtml]
		public string Content { get; set; }

	}
}