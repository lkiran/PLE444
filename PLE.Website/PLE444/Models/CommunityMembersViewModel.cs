using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PLE444.Models
{
    public class CommunityMembersViewModel
    {
        public Community CommunityInfo { get; set; }

        public List<UserViewModel> Users { get; set; }


    }
}