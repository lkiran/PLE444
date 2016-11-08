using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PLE444.Models
{
    public class UserAssignment
    {
        [Key]
        public Assignment Assignment { get; set; }
        [Key]
        public String UserId { get; set; }
        public Document Document { get; set; }
        public DateTime SubmitDate { get; set; }
    }
}