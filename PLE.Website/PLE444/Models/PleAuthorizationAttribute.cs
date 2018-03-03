using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using PLE.Website.Common;

namespace PLE444.Models {
	public class PleAuthorizationAttribute : AuthorizeAttribute {
		public string AccessLevel { get; set; }

		protected override bool AuthorizeCore(HttpContextBase httpContext) {
			return Common.Token != null;
		}

		protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext) {
			if (filterContext.HttpContext.Request.Url != null)
				filterContext.Result = new RedirectToRouteResult(
					new RouteValueDictionary(
						new {
							controller = "Account",
							action = "Login",
							returnUrl = filterContext.HttpContext.Request.Url.GetComponents(UriComponents.PathAndQuery,
								UriFormat.SafeUnescaped)
						})
				);
		}
	}
}