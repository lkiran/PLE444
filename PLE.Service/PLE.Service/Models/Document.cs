using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PLE.Service.Models
{
	public class Document
	{
        [Key]
        public int Id { get; set; }

	    public ApplicationUser Owner { get; set; }

        [ForeignKey("Owner")]
        public string OwnerId { get; set; }	

		public string FilePath { get; set; }

		public string Description { get; set; }

        public DateTime DateUpload { get; set; }
    }
}