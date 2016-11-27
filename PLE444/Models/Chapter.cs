using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PLE444.Models
{
    public class Chapter
    {
        public Chapter()
        {
            Id = Guid.NewGuid();
        }

        [Required]
        public Guid Id { get; private set; }

        public Guid CourseId { get; set; }

        [Required]
        [DisplayName("Başlık")]
        public String Title { get; set; }

        [Required]
        [AllowHtml]
        [DisplayName("İçerik")]
        public String Description { get; set; }
        [Required]
        
        public DateTime DateAdded { get; set; }

        public virtual ICollection<Material> Materials { get; set; }
        
    }
}