using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PLE444.Models
{
    public class UserViewModel
    {
        public string UserID { get; set; }
        public string ProfilePhoto { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public bool? isFriend { get; set; }

        public string FullName()
        {
            return Name + " " + Surname;
        }
    }
}