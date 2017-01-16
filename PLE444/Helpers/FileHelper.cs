using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

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

            return "/Content/img/FileIcons/" + fileType + ".png";
        }
    }
}