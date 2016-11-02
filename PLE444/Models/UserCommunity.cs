using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PLE444.Models
{
	public class UserCommunity
	{
		[Key]
		public String UserId { get; set; }
		[Key]
		public Community Community { get; set; }
		public DateTime ApprovalDate { get; set; }
	}
}