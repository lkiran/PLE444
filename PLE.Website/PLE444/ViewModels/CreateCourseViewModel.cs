using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace PLE444.ViewModels
{
	public class CreateCourseViewModel
	{
		public CreateCourseViewModel() {
			CanEveryoneJoin = false;
			IsCourseActive = true;
		}

		public Guid Id { get; set; }

		[Display(Name = "Dersin Kodu")]
		public string Code { get; set; }

		[Display(Name = "Dersin İsmi")]
		public string Name { get; set; }

		[AllowHtml]
		[Display(Name = "Açıklama")]
		public string Description { get; set; }

		public bool CanEveryoneJoin { get; set; }

		public bool IsCourseActive { get; set; }

		public string Heading => $"{Code} - {Name}";
	}
}