using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace PLE.Service.Models
{
    public class Assignment
    {      
        public Assignment()
        {
            Id = Guid.NewGuid();
            IsActive = true;
        }

        [Key]
        public Guid Id { get; set; }

        public bool IsActive { get; set; }

        public Course Course { get; set; }

        [Required]
        [ForeignKey("Course")]
        public Guid CourseId { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        [AllowHtml]
        public string Description { get; set; }

        [Required]
        public DateTime Deadline { get; set; }

        public DateTime DateAdded { get; set; }

        public virtual ICollection<Document> Uploads { get; set; }
    }
}