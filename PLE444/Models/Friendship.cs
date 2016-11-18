using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PLE444.Models
{
    public class Friendship
    {
        [Key]
        public String ID { get; set; }
        public String userID { get; set; }
        public String FriendID { get; set; }
        public Boolean isApproved { get; set; }
    }
}