using System.Collections.Generic;

namespace PLE.Service.Models
{
    public class SpaceViewModel
    {
        public Space SpaceInfo { get; set; }
        public List<Community> Communities { get; set; }

        public List<Course> Courses { get; set; }
    }
}