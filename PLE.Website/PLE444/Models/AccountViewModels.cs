using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;
using System.Web;

namespace PLE444.Models
{
	public class ExternalLoginConfirmationViewModel
	{
		[Required]
		[Display(Name = "Email")]

		public string Email { get; set; }
	}

	public class ExternalLoginListViewModel
	{
		public string ReturnUrl { get; set; }
	}

	public class SendCodeViewModel
	{
		public string SelectedProvider { get; set; }
		public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
		public string ReturnUrl { get; set; }
		public bool RememberMe { get; set; }
	}

	public class VerifyCodeViewModel
	{
		[Required]
		public string Provider { get; set; }

		[Required]
		[Display(Name = "Code")]
		public string Code { get; set; }
		public string ReturnUrl { get; set; }

		[Display(Name = "Remember this browser?")]
		public bool RememberBrowser { get; set; }

		public bool RememberMe { get; set; }
	}

	public class ForgotViewModel
	{
		[Required]
		[Display(Name = "Email")]
		public string Email { get; set; }
	}

	public class LoginViewModel
	{
		[Required(ErrorMessage ="E-Posta alanı boş bırakılamaz.")]
		[Display(Name = "E-Posta")]
		[EmailAddress(ErrorMessage = "Lütfen E-Posta adresinizi geçerli formata giriniz.")]
		public string Email { get; set; }

		[Required(ErrorMessage = "Şifre alanı boş bırakılamaz.")]
		[DataType(DataType.Password)]
		[Display(Name = "Şifre")]
		public string Password { get; set; }

		[Display(Name = "Beni Hatırla!")]
		public bool RememberMe { get; set; }
	}

	public class ResetPasswordViewModel
	{
		[Required(ErrorMessage = "E-Posta alanı boş bırakılamaz.")]
		[EmailAddress(ErrorMessage = "Lütfen E-Posta adresinizi geçerli formata giriniz.")]
		[Display(Name = "E-Posta")]
		public string Email { get; set; }

		[Required(ErrorMessage = "Şifre alanı boş bırakılamaz.")]
		[StringLength(100, ErrorMessage = " {0} en az {2} karakter uzunluğunda olmalıdır.", MinimumLength = 6)]
		[DataType(DataType.Password)]
		[Display(Name = "Şifre")]
		public string Password { get; set; }

		[DataType(DataType.Password)]
		[Display(Name = "Şifre Tekrar")]
		[Compare("Password", ErrorMessage = "Girilen şifreler eşleşmiyor")]
		public string ConfirmPassword { get; set; }

		public string Code { get; set; }
	}

	public class ForgotPasswordViewModel
	{
		[Required(ErrorMessage = "E-Posta alanı boş bırakılamaz.")]
		[EmailAddress(ErrorMessage = "Lütfen E-Posta adresinizi geçerli formata giriniz.")]
		[Display(Name = "Email")]
		public string Email { get; set; }
	}
}
