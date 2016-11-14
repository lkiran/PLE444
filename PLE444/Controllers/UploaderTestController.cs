using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PLE444.Controllers
{
	public class UploaderTestController : Controller
	{
		// GET: UploaderTest
		public ActionResult Index()
		{
			return View();
		}

		[HttpPost] //must be post method
        public ActionResult Index(IEnumerable<HttpPostedFileBase> files)
        {
            if (files == null)
                return RedirectToAction("Index", "Home");

			var path = "";  //for path to save

			foreach (var item in files)  //iterate in each file
			{
				if (item != null)   //check file is null or not
				{
					if (item.ContentLength > 0) //check length of bytes are greater then zero or not
					{
						//for checking uploaded file is image or not
						if (Path.GetExtension(item.FileName).ToLower() == ".jpg"
							|| Path.GetExtension(item.FileName).ToLower() == ".png"
						    || Path.GetExtension(item.FileName).ToLower() == ".gif"
							|| Path.GetExtension(item.FileName).ToLower() == ".jpeg")
						{
							path = Path.Combine(Server.MapPath("~/Uploads"), item.FileName);
							item.SaveAs(path);
							ViewBag.UploadSuccess = true;
						}
					}
				}
			}
			return View();
		}

	}
}