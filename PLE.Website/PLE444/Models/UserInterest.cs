using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PLE444.Models
{
	public class UserInterest
	{
		[Key]
		public String UserId { get; set; }
		[Key]
		public Course Course { get; set; }
	}
}