using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Security.Claims;
using System.Threading.Tasks;
using PLE.Contract.Enums;

namespace PLE.Service.Models
{
	// You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
	public class ApplicationUser : IdentityUser
	{
		public enum RoleType
		{
			NotSpecified = 0,
			Admin = 1,
			User = 2
		}
		
		public string FirstName { get; set; }

		public string LastName { get; set; }

		public GenderType Gender { get; set; }
		
		public string ProfilePicture { get; set; }

		public string PhoneNo { get; set; }

		public string Vision { get; set; }

		public string Mission { get; set; }

		public RoleType Role { get; set; }

		public string FullName => $"{FirstName} {LastName}";
		
		public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager) {
			// Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
			var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
			// Add custom user claims here
			return userIdentity;
		}

		public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager, string authenticationType) {
			var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
			// Add custom user claims here

			return userIdentity;
		}
	}
}