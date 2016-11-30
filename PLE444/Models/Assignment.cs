using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        public Guid Id { get; set; }
        public Course Course { get; set; }
        [Required]
        [DisplayName("Başlık")]
        public String Title { get; set; }
        [Required]
        [DisplayName("İçerik")]
        public String Description { get; set; }
        [Required]
        [DisplayName("Teslim Tarihi")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime Deadline { get; set; }
        public DateTime DateAdded { get; set; }

    }
}