using PLE.Service.Dto.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace PLE.Service.Dto
{
    public class RegisterUserDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
               
        public HttpPostedFileBase ProfilePicture { get; set; }
       
        public string FirstName { get; set; }
        
        public string LastName { get; set; }
        
        public GenderType Gender { get; set; }
        
        public string PhoneNo { get; set; }
        
        public string Vision { get; set; }
        
        public string Mission { get; set; }
    }
}
