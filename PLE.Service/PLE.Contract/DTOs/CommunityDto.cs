using System;
using System.Collections.Generic;


namespace PLE.Contract.DTOs
{
    public class CommunityDto
    {
        
        public Guid Id { get; set; }

        public string Name { get; set; }
        
        public string Description { get; set; }

        public DateTime DateCreated { get; set; }

        public bool IsActive { get; set; }

        public bool IsOpen { get; set; }

        public bool IsHiden { get; set; }
        
        public string OwnerId { get; set; }

        public UserDto Owner { get; set; }
        
        public int SpaceId { get; set; }

        public SpaceDto Space { get; set; }

        public virtual ICollection<DiscussionDto> Discussions { get; set; }
    
}
}
