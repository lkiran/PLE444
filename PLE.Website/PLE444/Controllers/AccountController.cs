using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using PLE444.Models;
using System.IO;
using Microsoft.Ajax.Utilities;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Mail;
using System.Security.Claims;
using System.Security.Principal;
using AutoMapper;
using PLE.Contract.DTOs;
using PLE.Website.Common;
using PLE.Website.Service;
using PLE444.ViewModels;
using static PLE444.Helpers.ViewHelper;

namespace PLE444.Controllers
{
	[Authorize]
	public class AccountController : Controller
	{
		private ApplicationSignInManager _signInManager;
		private ApplicationUserManager _userManager;
		private readonly AuthService _authService;

		public AccountController() {
			_authService = new AuthService();
		}

		public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager) {
			UserManager = userManager;
			SignInManager = signInManager;
		}

		public ApplicationSignInManager SignInManager {
			get { return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>(); }
			private set { _signInManager = value; }
		}

		public ApplicationUserManager UserManager {
			get { return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>(); }
			private set { _userManager = value; }
		}

		//
		// GET: /Account/Login
		[AllowAnonymous]
		public ActionResult Login(string returnUrl) {
			ViewBag.ReturnUrl = returnUrl;
			return View();
		}

		// POST: /Account/Login
		[HttpPost]
		[AllowAnonymous]
		public async Task<ActionResult> Login(LoginViewModel model, string returnUrl) {
			if (!ModelState.IsValid) return View(model);

			var result = SignInStatus.Failure;
			try {
				var token = _authService.GetAuthToken(model.Email, model.Password);
				_authService.UpdateClientToken(token);
				var user = _authService.GetActiveUser();
				user.Token = token;
				Common.Token = token;

				User.GetPrincipal().LoginUser(user);

				result = SignInStatus.Success;
				switch (result) {
					case SignInStatus.Success:
						if (model.RememberMe) 
							_authService.SetAuthCookie();
						
						return string.IsNullOrWhiteSpace(returnUrl) ? RedirectToAction("Index", "Home") : RedirectToLocal(returnUrl);

					case SignInStatus.LockedOut:
						return View("Lockout");

					case SignInStatus.RequiresVerification:
						return RedirectToAction("WaitingConfirmation", new { userId = user.Id });

					default:
						throw new Exception("Login Failure");
				}
			}
			catch (Exception e) {
				ModelState.AddModelError("", "Giriş yapılırken bir hata oluştu");
				return View(model);
			}
		}


		//
		// GET: /Account/VerifyCode
		[AllowAnonymous]
		public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe) {
			// Require that the user has already logged in via username/password or external login
			if (!await SignInManager.HasBeenVerifiedAsync()) {
				return View("Error");
			}
			return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
		}

		//
		// POST: /Account/VerifyCode
		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model) {
			if (!ModelState.IsValid) {
				return View(model);
			}

			// The following code protects for brute force attacks against the two factor codes. 
			// If a user enters incorrect codes for a specified amount of time then the user account 
			// will be locked out for a specified amount of time. 
			// You can configure the account lockout settings in IdentityConfig
			var result =
				await
					SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent: model.RememberMe,
						rememberBrowser: model.RememberBrowser);
			switch (result) {
				case SignInStatus.Success:
					return RedirectToLocal(model.ReturnUrl);
				case SignInStatus.LockedOut:
					return View("Lockout");
				case SignInStatus.Failure:
				default:
					ModelState.AddModelError("", "Invalid code.");
					return View(model);
			}
		}

		//
		// GET: /Account/Register
		[AllowAnonymous]
		public ActionResult Register() {
			return View(new RegisterViewModel());
		}

		//
		// POST: /Account/Register
		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public ActionResult Register(RegisterViewModel model) {
			if (!ModelState.IsValid) return View(model);

			var user = new UserDto {
				Email = model.Email,
				UserName = model.Email,
				Password = model.Password,
				FirstName = model.FirstName,
				LastName = model.LastName,
				PhoneNo = model.PhoneNo,
				Mission = model.Mission,
				Vision = model.Vision,
				Gender = model.Gender
			};
			if (!model.photoBase64.IsNullOrWhiteSpace()) {
				IList<string> data = model.photoBase64.Split(',').ToList();
				Debug.WriteLine(data[1]);
				byte[] bytes = Convert.FromBase64String(data[1]);
				var fileName = Guid.NewGuid() + "." + data[0].Split('/')[1].Split(';')[0];
				using (
					var imageFile = new FileStream(Path.Combine(Server.MapPath("~/Uploads"), fileName),
						FileMode.Create)) {
					imageFile.Write(bytes, 0, bytes.Length);
					imageFile.Flush();
				}
				user.ProfilePicture = "~/Uploads/" + fileName;
			}

			if (model.uploadFile != null && model.uploadFile.ContentLength > 0) {
				if (Path.GetExtension(model.uploadFile.FileName)?.ToLower() == ".jpg"
					|| Path.GetExtension(model.uploadFile.FileName)?.ToLower() == ".png"
					|| Path.GetExtension(model.uploadFile.FileName)?.ToLower() == ".gif"
					|| Path.GetExtension(model.uploadFile.FileName)?.ToLower() == ".jpeg") {
					var fileName = Guid.NewGuid() + Path.GetExtension(model.uploadFile.FileName);
					var imageFilePath = Path.Combine(Server.MapPath("~/Uploads"), fileName);
					model.uploadFile.SaveAs(imageFilePath);

					user.ProfilePicture = "~/Uploads/" + fileName;
				}
			}

			var result = _authService.RegisterUser(user);

			if (result.Status)
				return RedirectToAction("ResendConfirmation", new { userId = result.UserId });

			if (result.Errors.Any(e => e.Contains("already taken.")))
				ModelState.AddModelError("Email", "Bu e-posta adresi zaten kayıtlı");

			// If we got this far, something failed, redisplay form
			return View(model);
		}

		[AllowAnonymous]
		public ActionResult WaitingConfirmation(string userId) {
			ViewBag.CurrentUserId = userId;
			return View();
		}

		[AllowAnonymous]
		public async Task<ActionResult> ResendConfirmation(string userId) {

			//string code = await UserManager.GenerateEmailConfirmationTokenAsync(userId);
			//var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = userId, code = code }, protocol: Request.Url.Scheme);

			//var user = await UserManager.FindByIdAsync(userId);

			//var mail = new MailMessage {
			//	Subject = "Hesap Onayı",
			//	Body = ViewRenderer.RenderView("~/Views/Mail/ConfirmEmail.cshtml", new ViewDataDictionary()
			//		{
			//			{"confirmUrl", callbackUrl},
			//			{"userName", user.FullName()}
			//		}),
			//	IsBodyHtml = true
			//};

			//mail.Bcc.Add(user.Email);

			//await new EmailService().SendAsync(mail);

			return RedirectToAction("WaitingConfirmation", new { userId = userId });
		}

		//
		// GET: /Account/ConfirmEmail
		[AllowAnonymous]
		public async Task<ActionResult> ConfirmEmail(string userId, string code) {
			if (userId == null || code == null) {
				return View("Error");
			}
			var result = await UserManager.ConfirmEmailAsync(userId, code);
			return View(result.Succeeded ? "ConfirmEmail" : "Error");
		}

		//
		// GET: /Account/ForgotPassword
		[AllowAnonymous]
		public ActionResult ForgotPassword() {
			return View();
		}

		//
		// POST: /Account/ForgotPassword
		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model) {
			if (ModelState.IsValid) {
				var user = await UserManager.FindByNameAsync(model.Email);
				if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id))) {
					// Don't reveal that the user does not exist or is not confirmed
					return View("ForgotPasswordConfirmation", new { userId = user?.Id });
				}

				string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
				var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
				return RedirectToAction("ResendPassReset", "Account", new { userId = user.Id });
			}

			// If we got this far, something failed, redisplay form
			return View(model);
		}

		[AllowAnonymous]
		public async Task<ActionResult> ResendPassReset(string userId) {
			try {
				string code = await UserManager.GeneratePasswordResetTokenAsync(userId);
				var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = userId, code = code }, protocol: Request.Url.Scheme);

				var user = await UserManager.FindByIdAsync(userId);
				var mail = new MailMessage {
					Subject = "Parola Sıfırlama",
					Body = ViewRenderer.RenderView("~/Views/Mail/ResetPassword.cshtml", new ViewDataDictionary()
					{
						{"confirmUrl", callbackUrl},
						{"userName", user.FullName()}
					}),
					IsBodyHtml = true
				};

				mail.Bcc.Add(user.Email);

				await new EmailService().SendAsync(mail);
			}
			catch (Exception) {
				Debug.WriteLine("E-mail could not be sent");
			}

			return RedirectToAction("ForgotPasswordConfirmation", new { userId = userId });
		}
		//
		// GET: /Account/ForgotPasswordConfirmation
		[AllowAnonymous]
		public ActionResult ForgotPasswordConfirmation(string userId) {
			ViewBag.CurrentUserId = userId;
			return View();
		}

		//
		// GET: /Account/ResetPassword
		[AllowAnonymous]
		public ActionResult ResetPassword(string code) {
			return code == null ? View("Error") : View();
		}

		//
		// POST: /Account/ResetPassword
		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model) {
			if (!ModelState.IsValid) {
				return View(model);
			}
			var user = await UserManager.FindByNameAsync(model.Email);
			if (user == null) {
				// Don't reveal that the user does not exist
				return RedirectToAction("ResetPasswordConfirmation", "Account");
			}
			var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
			if (result.Succeeded) {
				return RedirectToAction("ResetPasswordConfirmation", "Account");
			}
			AddErrors(result);
			return View();
		}

		//
		// GET: /Account/ResetPasswordConfirmation
		[AllowAnonymous]
		public ActionResult ResetPasswordConfirmation() {
			return View();
		}

		//
		// POST: /Account/ExternalLogin
		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public ActionResult ExternalLogin(string provider, string returnUrl) {
			// Request a redirect to the external login provider
			return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
		}

		//
		// GET: /Account/SendCode
		[AllowAnonymous]
		public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe) {
			var userId = await SignInManager.GetVerifiedUserIdAsync();
			if (userId == null) {
				return View("Error");
			}
			var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
			var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
			return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
		}

		//
		// POST: /Account/SendCode
		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> SendCode(SendCodeViewModel model) {
			if (!ModelState.IsValid) {
				return View();
			}

			// Generate the token and send it
			if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider)) {
				return View("Error");
			}
			return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
		}

		//
		// GET: /Account/ExternalLoginCallback
		[AllowAnonymous]
		public async Task<ActionResult> ExternalLoginCallback(string returnUrl) {
			var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
			if (loginInfo == null) {
				return RedirectToAction("Login");
			}

			// Sign in the user with this external login provider if the user already has a login
			var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
			switch (result) {
				case SignInStatus.Success:
					return RedirectToLocal(returnUrl);
				case SignInStatus.LockedOut:
					return View("Lockout");
				case SignInStatus.RequiresVerification:
					return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
				case SignInStatus.Failure:
				default:
					// If the user does not have an account, then prompt the user to create an account
					ViewBag.ReturnUrl = returnUrl;
					ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
					return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
			}
		}

		//
		// POST: /Account/ExternalLoginConfirmation
		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl) {
			if (User.Identity.IsAuthenticated) {
				return RedirectToAction("Index", "Manage");
			}

			if (ModelState.IsValid) {
				// Get the information about the user from the external login provider
				var info = await AuthenticationManager.GetExternalLoginInfoAsync();
				if (info == null) {
					return View("ExternalLoginFailure");
				}
				var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
				var result = await UserManager.CreateAsync(user);
				if (result.Succeeded) {
					result = await UserManager.AddLoginAsync(user.Id, info.Login);
					if (result.Succeeded) {
						await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
						return RedirectToLocal(returnUrl);
					}
				}
				AddErrors(result);
			}

			ViewBag.ReturnUrl = returnUrl;
			return View(model);
		}

		//
		// POST: /Account/LogOff
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult LogOff() {
			_authService.DeleteAuthCookie();
			User.GetPrincipal().LogoutUser();
			return RedirectToAction("Index", "Home");
		}

		//
		// GET: /Account/ExternalLoginFailure
		[AllowAnonymous]
		public ActionResult ExternalLoginFailure() {
			return View();
		}

		protected override void Dispose(bool disposing) {
			if (disposing) {
				if (_userManager != null) {
					_userManager.Dispose();
					_userManager = null;
				}

				if (_signInManager != null) {
					_signInManager.Dispose();
					_signInManager = null;
				}
			}

			base.Dispose(disposing);
		}

		#region Helpers
		// Used for XSRF protection when adding external logins
		private const string XsrfKey = "XsrfId";

		private IAuthenticationManager AuthenticationManager {
			get {
				return HttpContext.GetOwinContext().Authentication;
			}
		}

		private void AddErrors(IdentityResult result) {
			foreach (var error in result.Errors) {
				ModelState.AddModelError("", error);
			}
		}

		private ActionResult RedirectToLocal(string returnUrl) {
			if (Url.IsLocalUrl(returnUrl)) {
				return Redirect(returnUrl);
			}
			return RedirectToAction("Index", "Home");
		}

		internal class ChallengeResult : HttpUnauthorizedResult
		{
			public ChallengeResult(string provider, string redirectUri)
				: this(provider, redirectUri, null) {
			}

			public ChallengeResult(string provider, string redirectUri, string userId) {
				LoginProvider = provider;
				RedirectUri = redirectUri;
				UserId = userId;
			}

			public string LoginProvider { get; set; }
			public string RedirectUri { get; set; }
			public string UserId { get; set; }

			public override void ExecuteResult(ControllerContext context) {
				var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
				if (UserId != null) {
					properties.Dictionary[XsrfKey] = UserId;
				}
				context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
			}
		}
		#endregion
	}
}