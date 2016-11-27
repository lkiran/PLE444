using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PLE444.Models
{
    public class Course
    {
        public Course()
        {
            ID = Guid.NewGuid();
        }

        [Required]
        public Guid ID { get; private set; }
        public String Name { get; set; }
        public String Description { get; set; }
        public DateTime CourseStart { get; set; }
        public virtual ICollection<Chapter> Chapters { get; set; }
        public virtual ICollection<Discussion> Discussion { get; set; }
    }
}