using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;
using PLE.Contract.DTOs;


namespace PLE444.Models
{
	public class UserPrincipal : IPrincipal
	{
		public UserDto User { get; }
		public IIdentity Identity { get; }

		public UserPrincipal(IIdentity identity, UserDto user = null) {
			if (user == null)
				user = (UserDto)HttpContext.Current.Session["User"];

			User = user;
			Identity = identity;
		}

		public bool IsInRole(string role) {
			return true;
		}
	}

	public static class IdentityManager
	{
		/// <summary>
		/// Setups current user's identity and domain principal.
		/// </summary>
		public static void SetupIdentity() {
			if (HttpContext.Current == null || HttpContext.Current.Session == null) return;
			if (HttpContext.Current.Session["Login"] == null || !(bool)HttpContext.Current.Session["Login"]) return;

			// Fetching claims and setting up our Identity.
			var claims = HttpContext.Current.Session["Claims"] as List<Claim> ?? new List<Claim>();
			var user = HttpContext.Current.Session["User"] as UserDto;

			if (user != null) {
				if (!claims.Any()) {
					claims.Add(new Claim(ClaimTypes.Name, user.FullName()));
					claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id));
				}
			}

			var identity = new ClaimsIdentity(claims,"PLE");
			HttpContext.Current.User = new UserPrincipal(identity, user);
		}
	}
}
