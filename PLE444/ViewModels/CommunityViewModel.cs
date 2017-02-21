using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PLE444.Models;

namespace PLE444.ViewModels
{
    public class CommunityViewModel
    {
        public Community Community { get; set; }

        public Enums.StatusType Status { get; set; }

        public int MemberCount { get; set; }
    }
}