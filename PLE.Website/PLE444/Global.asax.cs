using System;
using System.Diagnostics;
using System.Security.Claims;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using PLE.Website.Service;
using PLE444.Models;

namespace PLE444 {
	public class MvcApplication : System.Web.HttpApplication {
		protected void Application_Start() {
			AreaRegistration.RegisterAllAreas();
			FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
			RouteConfig.RegisterRoutes(RouteTable.Routes);
			BundleConfig.RegisterBundles(BundleTable.Bundles);
			BundleTable.EnableOptimizations = true;
			AntiForgeryConfig.UniqueClaimTypeIdentifier = ClaimTypes.NameIdentifier;
			IdentityManager.SetupIdentity();
		}

		protected void Application_AcquireRequestState(object sender, EventArgs e) {
			IdentityManager.SetupIdentity();
		}

		protected void Session_Start(object sender, EventArgs e) {
			try {
				using (var authService = new AuthService()) {
					var authToken = authService.GetAuthCookie();
					if (authToken == null) return;

					authService.UpdateClientToken(authToken);
					var user = authService.GetActiveUser();
					User.GetPrincipal().LoginUser(user);
				}
			} catch (Exception ex) {
				Debug.WriteLine($"User is not loged in on session start: {ex}");
			}
		}
	}
}
