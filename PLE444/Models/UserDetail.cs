using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PLE444.Models
{
	public class UserDetail
	{
        [Key]
        public String UserId { get; set; }
        public String Name { get; set; }
        [Required(ErrorMessage = "Lütfen öğrencinin soyadını yazınız")]
        public String Surname { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "E-Posta Adresi")]
        public string EPosta { get; set; }

        [Required]
        [Display(Name = "Doğum Tarihi")]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}")]
        public DateTime DogumTarih { get; set; }

        //[Required]
        //public int RoleId { get; set; }
        //public virtual Role Role { get; set; }
        public String PhoneNumber { get; set; }
        public String Gender { get; set; }
        [MinLength(100)]
        [MaxLength(1000)]
        public String Vision { get; set; }
        [MinLength(100)]
        [MaxLength(1000)]
        public String Mission { get; set; }
    }
}