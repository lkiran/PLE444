using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PLE444.Models
{
    public class Discussion
    {
        public class Reading
        {
            public int Id { get; set; }
            public string UserId { get; set; }
            public DateTime Date { get; set; }
        }

        public Discussion()
        {
            ID = Guid.NewGuid();
        }
        public Guid ID { get; set; }
        public string Topic { get; set; }
        public string CreatorId { get; set; }
        public DateTime DateCreated { get; set; }
        public virtual ICollection<Message> Messages { get; set; }
        public virtual ICollection<Reading> Readings { get; set; }
    }
}