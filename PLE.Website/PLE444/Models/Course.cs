using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PLE444.Models
{
    public class Course
    {
        public Course()
        {
            Id = Guid.NewGuid();
            CanEveryoneJoin = false;
        }

        [Required]
        public Guid Id { get; set; }

        [ForeignKey("Creator")]
        public string CreatorId { get; set; }

        public ApplicationUser Creator { get; set; }

        [Display(Name = "Dersin Kodu")]
        public string Code { get; set; }

        [Display(Name = "Dersin İsmi")]
        public string Name { get; set; }

        [AllowHtml]
        [Display(Name = "Açıklama")]
        public string Description { get; set; }

        public DateTime DateCreated { get; set; }

        public bool CanEveryoneJoin { get; set; }

        public Space Space { get; set; }

        [ForeignKey("Space")]
        public int SpaceId { get; set; }

        public virtual ICollection<Chapter> Chapters { get; set; }

        public virtual ICollection<Assignment> Assignments { get; set; }

        public virtual ICollection<Discussion> Discussion { get; set; }

        public virtual ICollection<TimelineEntry> Timeline { get; set; }

        public string Heading => Code + " - " + Name;
    }
}