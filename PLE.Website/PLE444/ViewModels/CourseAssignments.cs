using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PLE444.Models;

namespace PLE444.ViewModels
{
    public class CourseAssignments
    {
        public Course CourseInfo { get; set; }

        public bool CanEdit { get; set; }

        public bool CanUpload { get; set; }

        public string CurrentUserId { get; set; }

        public IEnumerable<Assignment> AssignmentList { get; set; }
    }
}