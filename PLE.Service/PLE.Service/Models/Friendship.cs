using System;
using System.ComponentModel.DataAnnotations;

namespace PLE.Service.Models
{
    public class Friendship
    {
        public Friendship()
        {
            Id = Guid.NewGuid();
        }

        [Required]
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public string FriendId { get; set; }
        public bool IsApproved { get; set; }
    }
}