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
	    public UserCourse()
	    {
	        IsActive = true;
	        DateJoin = null;
	    }

        [Key]
        public int Id { get; set; }

        public bool IsActive { get; set; }

	    public DateTime? DateJoin { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }

	    public ApplicationUser User { get; set; }

        [ForeignKey("Course")]
        public Guid CourseId { get; set; }

        public Course Course { get; set; }
	}
}