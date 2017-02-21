using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PLE444.Models
{
    public class PrivateMessage
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Sender")]
        public string SenderId { get; set; }

        public ApplicationUser Sender { get; set; }

        [ForeignKey("Receiver")]
        public string ReceiverId { get; set; }

        public ApplicationUser Receiver { get; set; }

        [AllowHtml]
        public string Content { get; set; }

        public DateTime DateSent { get; set; }

        public bool IsRead { get; set; }
    }
}