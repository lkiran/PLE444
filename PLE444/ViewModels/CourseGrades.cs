using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PLE444.Models;

namespace PLE444.ViewModels
{
    public class CourseGrades
    {
        public bool CanEdit { get; set; }

        public Course CourseInfo { get; set; }

        public IEnumerable<UserCourse> CourseUsers { get; set; }

        public IEnumerable<GradeType> GradeTypes { get; set; }

        public IEnumerable<UserGrade> UserGrades { get; set; }
    }
}