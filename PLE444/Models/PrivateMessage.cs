using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PLE444.Models
{
    public class PrivateMessage
    {
        [Key]
        public int Id { get; set; }

        public string SenderId { get; set; }

        public string ReceiverId { get; set; }

        [AllowHtml]
        public string Content { get; set; }

        public DateTime DateSent { get; set; }

        public bool isRead { get; set; }
    }
}