using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PLE444.Controllers
{
    public class FileController : Controller
    {
        public FileResult Download(string path,string name)
        {
            path = path.Replace(" ", "");
            byte[] fileBytes = System.IO.File.ReadAllBytes(Server.MapPath(path));
                               
            string fileName = name;
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);

        }
    }
}