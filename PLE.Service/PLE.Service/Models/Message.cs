using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace PLE.Service.Models
{
    public class Message
    {
        public Message()
        {
            Id = Guid.NewGuid();
        }
        public Guid Id { get; set; }

        [AllowHtml]
        public string Content { get; set; }

        [ForeignKey("Sender")]
        public string SenderId { get; set; }

        public ApplicationUser Sender { get; set; }

        public DateTime DateSent { get; set; }
    }
}