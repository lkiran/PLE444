using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PLE444.Models
{
    public class Material
    {
        public Material()
        {
            Id = Guid.NewGuid();
            Documents = new Collection<Document>();
        }

        [Required]
        public Guid Id { get; private set; }

        [Required]
        [DisplayName("Başlık")]
        public String Title { get; set; }

        [Required]
        [AllowHtml]
        [DisplayName("İçerik")]
        public String Description { get; set; }

        public DateTime DateAdded { get; set; }

        public virtual ICollection<Document> Documents { get; set; }
    }
}