using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PLE444.Models
{
    public class Message
    {
        public Message()
        {
            ID = Guid.NewGuid();
        }
        public Guid ID { get; set; }
        public string Content { get; set; }
        public string SenderId { get; set; }
        public DateTime DateSent { get; set; }
    }
}