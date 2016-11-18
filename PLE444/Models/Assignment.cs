using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PLE444.Models
{
    public class Assignment
    {
        public Assignment()
        {
            Id = Guid.NewGuid();
        }

        [Required]
        public Guid Id { get; private set; }
        public Course Course { get; set; }
        public String Title { get; set; }
        public String Description { get; set; }
        public DateTime Deadline { get; set; }
        public DateTime DateAdded { get; set; }

    }
}