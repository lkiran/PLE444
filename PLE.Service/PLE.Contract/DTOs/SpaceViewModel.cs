using System.Collections.Generic;

namespace PLE.Contract.DTOs
{
    public class SpaceViewModel
    {
        public Space SpaceInfo { get; set; }

        public List<Community> Communities { get; set; }

        public List<Course> Courses { get; set; }
    }
}