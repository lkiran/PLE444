using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PLE444.Models;

namespace PLE444.ViewModels
{
    public class Chapters
    {
        public bool canEdit { get; set; }
        public Course CourseInfo { get; set; }
        public IEnumerable<Chapter> ChapterList { get; set; }        
    }
}