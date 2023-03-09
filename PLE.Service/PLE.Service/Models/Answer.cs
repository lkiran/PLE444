using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace PLE.Service.Models
{
	public class Answer
	{
		public Answer() {
			Id = new Guid();
		}

		[Key]
		public Guid Id { get; set; }

		[AllowHtml]
		public string Content { get; set; }
	}
}