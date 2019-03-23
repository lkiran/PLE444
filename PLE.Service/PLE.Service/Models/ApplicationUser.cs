using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel;
using System.Security.Claims;
using System.Threading.Tasks;
using PLE.Contract.Enums;

namespace PLE.Service.Models
{
	// You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
	public class ApplicationUser : IdentityUser
	{
		public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager) {
			// Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
			var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
			// Add custom user claims here
			return userIdentity;
		}
		[DisplayName("Ad")]
		public string FirstName { get; set; }

		[DisplayName("Soyad")]
		public string LastName { get; set; }

		[DisplayName("Cinsiyet")]
		public GenderType Gender { get; set; }
		public string ProfilePicture { get; set; }

		[DisplayName("Telefon")]
		public string PhoneNo { get; set; }

		[DisplayName("Vizyon")]
		public string Vision { get; set; }

		[DisplayName("Misyon")]
		public string Mission { get; set; }

		public string FullName => $"{FirstName} {LastName}";

		public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager, string authenticationType) {
			var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
			// Add custom user claims here
			return userIdentity;
		}
	}
}