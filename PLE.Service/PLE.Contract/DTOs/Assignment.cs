using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PLE.Contract.DTOs
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
        public Guid CourseId { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime Deadline { get; set; }

        public DateTime DateAdded { get; set; }

        public virtual ICollection<Document> Uploads { get; set; }
    }
}