using System;
using PLE444.Models;
using Microsoft.Owin;
using System.Net.Mail;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Owin.Security;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity.EntityFramework;

namespace PLE444 {
	public class EmailService : IIdentityMessageService, IDisposable {
		public Task SendAsync(IdentityMessage message) {
			var client = new SmtpClient {
				Host = "atmaca.cc.boun.edu.tr",
				Credentials = new System.Net.NetworkCredential("cet", "4M36xo"),
				Port = 25,
				EnableSsl = true
			};

			const string sentFrom = "cet@boun.edu.tr";

			// Create the message:
			var mail = new MailMessage(sentFrom, message.Destination) {
				Subject = message.Subject,
				Body = message.Body
			};

			return client.SendMailAsync(mail);
		}

		public Task SendAsync(MailMessage message) {
			var client = new SmtpClient {
				Host = "atmaca.cc.boun.edu.tr",
				Credentials = new System.Net.NetworkCredential("cet", "4M36xo"),
				Port = 25,
				EnableSsl = true
			};

			message.From = new MailAddress("cet@boun.edu.tr");
			message.IsBodyHtml = true;

			return client.SendMailAsync(message);
		}

		public void Dispose() {
			throw new NotImplementedException();
		}
	}

	public class SmsService : IIdentityMessageService {
		public Task SendAsync(IdentityMessage message) {
			// Plug in your SMS service here to send a text message.
			return Task.FromResult(0);
		}
	}

	// Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.
	public class ApplicationUserManager : UserManager<ApplicationUser> {
		public ApplicationUserManager(IUserStore<ApplicationUser> store)
			: base(store) {
		}

		public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context) {
			var manager = new ApplicationUserManager(new UserStore<ApplicationUser>(context.Get<PleDbContext>()));
			// Configure validation logic for usernames
			manager.UserValidator = new UserValidator<ApplicationUser>(manager) {
				AllowOnlyAlphanumericUserNames = false,
				RequireUniqueEmail = true
			};

			// Configure validation logic for passwords
			manager.PasswordValidator = new PasswordValidator {
				RequiredLength = 6,
				//RequireNonLetterOrDigit = true,
				//RequireDigit = true,
				//RequireLowercase = true,
				//RequireUppercase = true,
			};

			// Configure user lockout defaults
			manager.UserLockoutEnabledByDefault = true;
			manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromHours(1);
			manager.MaxFailedAccessAttemptsBeforeLockout = 5;

			// Register two factor authentication providers. This application uses Phone and Emails as a step of receiving a code for verifying the user
			// You can write your own provider and plug it in here.
			manager.RegisterTwoFactorProvider("Phone Code", new PhoneNumberTokenProvider<ApplicationUser> {
				MessageFormat = "Your security code is {0}"
			});
			manager.RegisterTwoFactorProvider("Email Code", new EmailTokenProvider<ApplicationUser> {
				Subject = "Security Code",
				BodyFormat = "Your security code is {0}"
			});
			manager.EmailService = new EmailService();
			manager.SmsService = new SmsService();
			var dataProtectionProvider = options.DataProtectionProvider;
			if (dataProtectionProvider != null) {
				manager.UserTokenProvider =
					new DataProtectorTokenProvider<ApplicationUser>(dataProtectionProvider.Create("ASP.NET Identity"));
			}
			return manager;
		}
	}

	// Configure the application sign-in manager which is used in this application.
	public class ApplicationSignInManager : SignInManager<ApplicationUser, string> {
		public ApplicationSignInManager(ApplicationUserManager userManager, IAuthenticationManager authenticationManager)
			: base(userManager, authenticationManager) {
		}

		public override Task<ClaimsIdentity> CreateUserIdentityAsync(ApplicationUser user) {
			return user.GenerateUserIdentityAsync((ApplicationUserManager)UserManager);
		}

		public static ApplicationSignInManager Create(IdentityFactoryOptions<ApplicationSignInManager> options, IOwinContext context) {
			return new ApplicationSignInManager(context.GetUserManager<ApplicationUserManager>(), context.Authentication);
		}
	}
}
