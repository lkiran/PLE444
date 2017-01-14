using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PLE444.Models
{
    public class GradeType
    {
        public GradeType()
        {
            MaxScore = 100;
            Effect = 100;
        }

        public int Id { get; set; }

        [Required]
        [ForeignKey("Course")]
        public Guid CourseId { get; set; }

        public virtual Course Course { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        [Range(0, 100)]
        public int Effect { get; set; }

        [Required]
        public float MaxScore { get; set; }
    }
}
