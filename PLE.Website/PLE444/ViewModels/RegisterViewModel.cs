using System.ComponentModel.DataAnnotations;
using System.Web;
using PLE.Contract.Enums;

namespace PLE444.ViewModels
{
	public class RegisterViewModel
	{
		public RegisterViewModel() {
			Gender = GenderType.NotSepecified;
		}

		[Required (ErrorMessage = "Email alanı boş bırakılmaz")]
		[EmailAddress]
		[Display(Name = "E-Posta")]
		public string Email { get; set; }

		[Required (ErrorMessage = "Şifre alanı zorunludur")]
		[StringLength(100, ErrorMessage = "Şifre en kısa {0} ve en uzun {2} karakter olabilir", MinimumLength = 6)]
		[DataType(DataType.Password)]
		[Display(Name = "Şifre")]
		public string Password { get; set; }

		[DataType(DataType.Password)]
		[Display(Name = "Şifre Tekrar")]
		[Compare("Password", ErrorMessage = "Şifreler eşleşmiyor!")]
		public string ConfirmPassword { get; set; }

		[Display(Name = "Profil Resmi")]
		public HttpPostedFileBase uploadFile { get; set; }

		public string photoBase64 { get; set; }

        [Required (ErrorMessage= "İsim alanı gereklidir")]
		[Display(Name = "İsim")]
		public string FirstName { get; set; }

        [Required (ErrorMessage = "Soyisim alanı gerklidir")]
		[Display(Name = "Soyisim")]
		public string LastName { get; set; }

		[Display(Name = "Cinsiyet")]
		public GenderType Gender { get; set; }

		[Display(Name = "Telefon Numarası")]
		public string PhoneNo { get; set; }

		[Display(Name = "Vizyon")]
		public string Vision { get; set; }

		[Display(Name = "Misyon")]
		public string Mission { get; set; }
	}
}