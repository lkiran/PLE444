using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace PLE.Service.Models
{
    public class Material
    {
        public Material()
        {
            Id = Guid.NewGuid();
            IsActive = true;
        }

        [Key]
        public Guid Id { get; set; }

        public bool IsActive { get; set; }

        public ApplicationUser Owner { get; set; }

        [ForeignKey("Owner")]
        public string OwnerId { get; set; }

        [Required]
        [DisplayName("Başlık")]
        public string Title { get; set; }

        [Required]
        [AllowHtml]
        [DisplayName("İçerik")]
        public string Description { get; set; }

        public DateTime DateAdded { get; set; }

        public virtual ICollection<Document> Documents { get; set; }
    }
}