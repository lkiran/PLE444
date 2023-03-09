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
    public class AssignmentForm
    {
        public Guid? Id { get; set; }

        public Course Course { get; set; }

        [Required]
        [HiddenInput]
        [ForeignKey("Course")]
        public Guid CourseId { get; set; }

        [Required]
        [DisplayName("Başlık")]
        public string Title { get; set; }

        [Required]
        [AllowHtml]
        [DisplayName("İçerik")]
        public string Description { get; set; }

        [Required]
        [DisplayName("Teslim Tarihi")]
        public DateTime Deadline { get; set; }

        public bool IsHidden { get; set; }
    }
}