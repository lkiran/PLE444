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
        public Guid ID { get; set; }
        public string CreatorId { get; set; }
        [Display(Name = "Dersin İsmi")]
        public String Name { get; set; }
        [Display(Name = "Dersin Tanımı")]
        public String Description { get; set; }
        [Display(Name = "Başlama Tarihi")]
        public DateTime CourseStart { get; set; }
        public virtual ICollection<Chapter> Chapters { get; set; }
        public virtual ICollection<Assignment> Assignments { get; set; }
        public virtual ICollection<Discussion> Discussion { get; set; }

        public int SpaceId { get; set; }

        public string Heading { get { return Name + " - " + Description; } }
    }
}