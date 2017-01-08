using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PLE444.Models
{


    public class CourseMembersViewModel
    {        
        public Course CourseInfo { get; set; }

        public List<ApplicationUser> Users { get; set; }

        public List<GradeType> GradeTypes { get; set; }

        public List<UserGrade> UserGrades { get; set; }

        public bool isCreator { get; set; }
    }
}