using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PLE444.Models
{
    public class SpaceViewModel
    {
        public Space SpaceInfo { get; set; }
        public List<Community> Communities { get; set; }

        public List<Course> Courses { get; set; }
    }
}