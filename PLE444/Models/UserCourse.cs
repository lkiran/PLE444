using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PLE444.Models
{
	public class UserCourse
	{
		[Key]
        public int Id { get; set; }
        public String UserId { get; set; }    
		public Course Course { get; set; }
		public DateTime? ApprovalDate { get; set; }
	}
}