using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PLE444.Models;

namespace PLE444.ViewModels
{
    public class CourseViewModel
    {
        public Course Course { get; set; }

        public bool IsCourseCreator { get; set; }

        public bool IsMember { get; set; }

        public int MemberCount { get; set; }
    }
}