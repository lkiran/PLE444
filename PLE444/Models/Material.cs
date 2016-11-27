using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PLE444.Models
{
    public class Material
    {
        public Material()
        {
            Id = Guid.NewGuid();
        }

        [Required]
        public Guid Id { get; private set; }

        [Required]
        [DisplayName("Başlık")]
        public String Title { get; set; }

        [Required]
        [DisplayName("İçerik")]
        public String Description { get; set; }

        public DateTime DateAdded { get; set; }

        public ICollection<Document> Document { get; set; }
    }
}