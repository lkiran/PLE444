using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using PLE444.Models;
//using Ionic.Zip;
using System.Diagnostics;

namespace PLE444.Controllers
{
    public class FileController : Controller
    {
        private PleDbContext db = new PleDbContext();

        public ActionResult Download(string path,string name)
        {
            path = path.Replace(" ", "");

            if (!System.IO.File.Exists(Server.MapPath(path)))
                return HttpNotFound();

            var fileBytes = System.IO.File.ReadAllBytes(Server.MapPath(path));

            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, name);
        }
    }
}