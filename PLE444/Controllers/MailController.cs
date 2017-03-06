using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace PLE444.Controllers
{
    public class MailController : Controller
    {
        public ActionResult NewMessage()
        {
            return View();
        }

        public ActionResult NewAssignment()
        {
            return View();
        }

        public ActionResult ConfirmEmail()
        {
            return View();
        }

        public ActionResult ResetPassword()
        {
            return View();
        }

        public static string RenderViewToString(string viewName, ViewDataDictionary viewData)
        {
            using (var writer = new StringWriter())
            {
                var routeData = new RouteData();
                routeData.Values.Add("controller", "Mail");
                var mailControllerContext = new ControllerContext(new HttpContextWrapper(new HttpContext(new HttpRequest(null, "http://google.com", null), new HttpResponse(null))), routeData, new MailController());
                var razorViewEngine = new RazorViewEngine();
                var razorViewResult = razorViewEngine.FindView(mailControllerContext, viewName, "", false);

                var viewContext = new ViewContext(mailControllerContext, razorViewResult.View, new ViewDataDictionary(viewData), new TempDataDictionary(), writer);
                razorViewResult.View.Render(viewContext, writer);
                return writer.ToString();

            }
        }
    }
}