using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;
using PLE.Contract.DTOs;

namespace PLE444.Models
{
	public static class PleUserIdentity
	{
		public static UserPrincipal GetPrincipal(this IPrincipal principal) {
			return principal as UserPrincipal;
		}

		public static void LoginUser(this IPrincipal principal, UserDto user) {
			if (HttpContext.Current == null || HttpContext.Current.Session == null)
				throw new ArgumentNullException("HttpContext is null");

			UpdateUser(principal, user);
			HttpContext.Current.Session["Login"] = true;
			HttpContext.Current.Session["User"] = user;
		}

		public static void LogoutUser(this IPrincipal principal) {
			if (HttpContext.Current == null || HttpContext.Current.Session == null)
				throw new ArgumentNullException("HttpContext is null!");

			UpdateUser(principal, null);
			HttpContext.Current.Session["Login"] = false;
			HttpContext.Current.Session["User"] = null;
		}

		public static void UpdateUser(this IPrincipal principal, UserDto user) {
			HttpContext.Current.Session["User"] = user;
		}
	}
}