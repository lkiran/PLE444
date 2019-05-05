using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PLE444.Models
{
    public class Discussion
    {
        public class Reading
        {
            public int Id { get; set; }

            [ForeignKey("User")]
            public string UserId { get; set; }

            public ApplicationUser User { get; set; }

            public DateTime Date { get; set; }
        }

        public Discussion()
        {
            ID = Guid.NewGuid();
        }

        public Guid ID { get; set; }
		[Required(ErrorMessage = "Tartışma konusu girilmesi zorunludur")]
		[DisplayName("Konu")]
		public string Topic { get; set; }

        [ForeignKey("Creator")]
        public string CreatorId { get; set; }

        public ApplicationUser Creator { get; set; }

        public DateTime DateCreated { get; set; }

        public virtual ICollection<Message> Messages { get; set; }
        
        public virtual ICollection<Reading> Readings { get; set; }
    }
}