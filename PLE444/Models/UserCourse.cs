using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PLE444.Models
{
	public class UserCourse
	{
		[Key]
        public int Id { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }

	    public ApplicationUser User { get; set; }

        [ForeignKey("Course")]
        public Guid CourseId { get; set; }

        public Course Course { get; set; }

		public DateTime? ApprovalDate { get; set; }
	}
}