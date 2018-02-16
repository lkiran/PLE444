using System;
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
}