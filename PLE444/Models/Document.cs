using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PLE444.Models
{
	public class Document
	{
        [Key]
        public int Id { get; set; }

        public String Owner { get; set; }	

		public String FilePath { get; set; }

		public String Description { get; set; }

        public DateTime DateUpload { get; set; }
    }
}