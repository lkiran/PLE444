using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PLE444.Models
{
    public class GradeType
    {
        public GradeType()
        {
            MaxScore = 100;
            Effect = 1f;
        }

        public int Id { get; set; }
        [Required]
        public Course Course { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required]
        public float Effect { get; set; }
        [Required]
        public float MaxScore { get; set; }
    }
}
