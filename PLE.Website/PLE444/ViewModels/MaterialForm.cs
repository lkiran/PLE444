using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PLE444.Models;

namespace PLE444.ViewModels
{
    public class MaterialForm
    {
        public MaterialForm()
        {
            Documents = new List<Document>();
            UploadedFiles= new List<HttpPostedFileBase>();
        }

        public Guid ChapterId { get; set; }

        public Guid? Id { get; set; }

        [DisplayName("Başlık")]
        public string Title { get; set; }

        public string OwnerId { get; set; }

        [AllowHtml]
        [DisplayName("Açıklama")]
        public string Description { get; set; }

        public List<Document> Documents { get; set; }

        public List<HttpPostedFileBase> UploadedFiles { get; set; }

        public DateTime Activate { get; set; }
    }
}