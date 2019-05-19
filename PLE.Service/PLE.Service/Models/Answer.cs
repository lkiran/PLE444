using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace PLE.Service.Models
{
	public class Answer
	{
		[Key]
		public Guid Id { get; set; }

		[AllowHtml]
		public string Content { get; set; }
	}
}