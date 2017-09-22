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
				user = (UserDto)HttpContext.Current.Session["AttemptedUser"];

			User = user;
			Identity = identity;
		}

		public bool IsInRole(string role) {
			return true;
		}
	}
}
