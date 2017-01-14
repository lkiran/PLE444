using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PLE444.Models;

namespace PLE444.ViewModels
{
    public class CourseMaterials
    {
        public Guid CourseId { get; set; }

        public IEnumerable<Chapter> ChapterList { get; set; }

        public bool CanEdit { get; set; }
    }
}