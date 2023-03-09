using System;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Owin;
using PLE.Service.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity.EntityFramework;

namespace PLE.Service.App_Start
{
	public class ApplicationUserManager : UserManager<ApplicationUser>
	{
		public ApplicationUserManager(IUserStore<ApplicationUser> store) : base(store) { }

		public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context)
		{
			var appDbContext = context.Get<PleDbContext>();
			
			var manager = new ApplicationUserManager(new UserStore<ApplicationUser>(appDbContext)){
				UserTokenProvider = new DataProtectorTokenProvider<ApplicationUser>(options.DataProtectionProvider.Create("ASP.NET Identity")){
					TokenLifespan = TimeSpan.FromDays(1)
				}
			};
			
		
			return manager;
		}
	}

	public class EmailService : IIdentityMessageService, IDisposable {
		#region Fields
		public string SenderAddress { get; set; } = "cet@boun.edu.tr";

		private readonly SmtpClient _client;
		#endregion

		#region Ctor
		public EmailService() {
			_client = new SmtpClient {
				Host = "atmaca.cc.boun.edu.tr",
				Credentials = new System.Net.NetworkCredential("cet", "4M36xo"),
				Port = 25,
				EnableSsl = true
			};
		} 
		#endregion

		public Task SendAsync(IdentityMessage message) {
			var mail = new MailMessage(SenderAddress, message.Destination) {
				Subject = message.Subject,
				Body = message.Body
			};

			return _client.SendMailAsync(mail);
		}

		public Task SendAsync(MailMessage message) {
			message.From = new MailAddress(SenderAddress);
			message.IsBodyHtml = true;

			return _client.SendMailAsync(message);
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

}