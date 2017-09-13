using System.Web.Http;
using System.Web.Http.Cors;

namespace PLE.Service
{
	public static class WebApiConfig
	{
		public static void Register(HttpConfiguration config)
		{
			var cors = new EnableCorsAttribute("http://localhost:50033", "*", "*");
			config.EnableCors(cors);
		}
	}
}