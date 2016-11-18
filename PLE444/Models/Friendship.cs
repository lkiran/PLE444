using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PLE444.Models
{
    public class Friendship
    {
        public Friendship()
        {
            Id = Guid.NewGuid();
        }

        [Required]
        public Guid Id { get; private set; }
        public String userID { get; set; }
        public String FriendID { get; set; }
        public Boolean isApproved { get; set; }
    }
}