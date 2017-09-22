using System.Security.Claims;
using System.Web;
using PLE444.Models.Interfaces;

namespace PLE444.Models
{
	public class UserSession : IUserSession
	{
		public string Email => ((ClaimsPrincipal)HttpContext.Current.User).FindFirst(ClaimTypes.Email).Value;

		public string BearerToken => ((ClaimsPrincipal)HttpContext.Current.User).FindFirst("AcessToken").Value;
	}
}