﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PLE444.Models
{
    public class GradeType
    {
        public GradeType()
        {
            MaxScore = 100;
            Effect = 100;
            IsActive = true;
        }

        public int Id { get; set; }

        public bool IsActive { get; set; }

        [Required]
        [ForeignKey("Course")]
        public Guid CourseId { get; set; }
		
        public virtual Course Course { get; set; }

		[Required(ErrorMessage = "İsim alanı boş bırakılamaz.")]
		[DisplayName("İsim")]
        public string Name { get; set; }

        [DisplayName("Açıklama")]
        public string Description { get; set; }

        [Required]
        [Range(0, 100)]
        [DisplayName("Yüzdelik Etki")]
        public int Effect { get; set; }

        [Required]
        [DisplayName("En Yüksek Puan")]
        public float MaxScore { get; set; }
    }
}
