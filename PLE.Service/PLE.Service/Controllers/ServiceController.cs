using System.Net;
using System.Web.Mvc;

namespace PLE.Service.Controllers
{
	public class ServiceController : Controller
	{
		// GET: Service
		public ActionResult Index()
		{
			return new HttpStatusCodeResult(HttpStatusCode.OK);
		}
	}
}