using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PLE444.Helpers
{
    public class FileHelper
    {
        public string GetFileIcon(string filePath)
        {
            var fileType = Path.GetExtension(filePath);

            fileType = fileType.Replace(".", "");
            fileType = fileType.Trim();
            fileType = fileType.ToLower();
            if (fileType == "")
                fileType = "unknown";

            var iconPath = "~/Content/img/FileIcons/" + fileType + ".png";

            return File.Exists(System.Web.HttpContext.Current.Server.MapPath(iconPath)) ? iconPath : "~/Content/img/FileIcons/unknown.png";
        }
    }
}