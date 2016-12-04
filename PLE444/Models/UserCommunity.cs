using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PLE444.Models
{
	public class UserCommunity
	{
        public UserCommunity()
        {
            Id = Guid.NewGuid();
        }

        [Required]
        public Guid Id { get; set; }

        public string UserId { get; set; }
        
        public virtual Community Community { get; set; }

        public DateTime DateJoined { get; set; }
	}
}