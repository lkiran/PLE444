using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PLE444.Models
{
    public class Discussion
    {
        public Discussion()
        {
            ID = Guid.NewGuid();
        }
        public Guid ID { get; set; }
        public string Topic { get; set; }
        public Guid CreatorId { get; set; }
        public DateTime DateCreated { get; set; }
        public virtual ICollection<Message> Messages { get; set; }
    }
}