using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace PLE.Service.Models
{
    public class TimelineEntry
    {
        public TimelineEntry()
        {
            Id = Guid.NewGuid();
            ColorClass = "timeline-primary";
            IconClass = "ti ti-plus";
        }

        public Guid Id { get; set; }

        public ApplicationUser Creator { get; set; }

        [ForeignKey("Creator")]
        public string CreatorId { get; set; }

        public DateTime DateCreated { get; set; }

        public string Heading { get; set; }

        [AllowHtml]
        public string Content { get; set; }

        public string ColorClass { get; set; }

        public string IconClass { get; set; }
    }
}