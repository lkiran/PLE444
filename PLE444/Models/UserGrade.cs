using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PLE444.Models
{
    public class UserGrade
    {
        [Key]
        public int Id { get; set; }
        public string UserID { get; set; }
        public GradeType GradeType { get; set; }
        public float Grade { get; set; }
    }
}