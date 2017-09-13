using System.Threading.Tasks;
using PLE.Website.Service;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace PLE444.Controllers
{
	public class TestController : Controller
	{
		private AuthService auth = new AuthService();
		// GET: Test
		public async Task<ActionResult> Index()
		{
			var result = await auth.Login("","");

			return Content(result);
		}
	}
}