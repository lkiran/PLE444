using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PLE444.Models
{
	public class UserDetail
	{
		[Key]
		public String UserId { get; set; }
		public String Name { get; set; }
		public String Surname { get; set; }
		public String About { get; set; }

		public Role Role { get; set; }
	}
}