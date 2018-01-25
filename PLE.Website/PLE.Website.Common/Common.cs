using System;
using System.Web;
using PLE.Contract.DTOs;

namespace PLE.Website.Common
{
	public static class Common
	{
		public static TokenDto Token {
			get {
				if (HttpContext.Current == null || HttpContext.Current.Session == null)
					return null;

				var user = (UserDto)HttpContext.Current.Session["User"];
				return user?.Token;
			}
			set {
				if (HttpContext.Current == null || HttpContext.Current.Session == null)
					throw new Exception("Can't reach to the session");
				var user = (UserDto)HttpContext.Current.Session["User"] ?? new UserDto();
				user.Token = value;
				HttpContext.Current.Session["User"] = user;
			}
		}
	}
}
