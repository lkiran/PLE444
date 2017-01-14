using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using PLE444.Models;

namespace PLE444.Controllers
{
    public class FileController : Controller
    {
        private PleDbContext db = new PleDbContext();

        public FileResult Download(string path,string name)
        {
            path = path.Replace(" ", "");
            byte[] fileBytes = System.IO.File.ReadAllBytes(Server.MapPath(path));
                               
            string fileName = name;
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);

        }

        //public List<Document> SaveFiles(IEnumerable<HttpPostedFileBase> uploadedFiles)
        //{
        //    var docs = new List<Document>();

        //    foreach (var item in uploadedFiles)  //iterate in each file
        //    {
        //        if (item != null && item.ContentLength > 0) //check length of bytes are greater then zero or not
        //        {
        //            var fileName = "";
        //            fileName = Guid.NewGuid().ToString() + Path.GetExtension(item.FileName);
        //            var imageFilePath = Path.Combine(Server.MapPath("~/Uploads"), fileName);
        //            item.SaveAs(imageFilePath);
        //            ViewBag.UploadSuccess = true;

        //            //Add to DB
        //            var d = new Document
        //            {
        //                FilePath = "/ Uploads / " + fileName,
        //                Owner = User.Identity.GetUserId(),
        //                DateUpload = DateTime.Now,
        //                Description = item.FileName
        //            };

        //            var doc = db.Documents.Add(d);
        //            docs.Add(doc);
        //        }
        //    }

        //    db.SaveChanges();
        //    return docs;
        //}

        //public Document SaveFile(HttpPostedFileBase uploadedFile)
        //{
        //    Document doc = null;

        //    if (uploadedFile != null && uploadedFile.ContentLength > 0) //check length of bytes are greater then zero or not
        //    {
        //        var fileName = "";
        //        fileName = Guid.NewGuid().ToString() + Path.GetExtension(uploadedFile.FileName);
        //        var imageFilePath = Path.Combine(Server.MapPath("~/Uploads"), fileName);
        //        uploadedFile.SaveAs(imageFilePath);
        //        ViewBag.UploadSuccess = true;

        //        //Add to DB
        //        doc = new Document
        //        {
        //            FilePath = "/ Uploads / " + fileName,
        //            Owner = User.Identity.GetUserId(),
        //            DateUpload = DateTime.Now,
        //            Description = uploadedFile.FileName
        //        };

        //        doc = db.Documents.Add(doc);
        //        db.SaveChanges();
        //    }

        //    return doc;
        //}
    }
}