using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PLE444.Models
{
	public class Document
	{
		public String Owner { get; set; }
		[Key]
		public String FilePath { get; set; }
		public String Description { get; set; }

        public DateTime DateUpload { get; set; }
    }
}