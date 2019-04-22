using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PLE444.Models;

namespace PLE444.ViewModels
{
    public class CourseViewModel
    {
        public Course Course { get; set; }

        public bool IsCourseCreator { get; set; }

        public bool IsMember { get; set; }

        public bool IsWaiting { get; set; }

        public int MemberCount { get; set; }
    }

    public class CourseMembers
    {
        public List<UserCourse> Members { get; set; }

        public bool CanEdit { get; set; }

        public Course Course { get; set; }
	}
	public class CourseCreateViewModel
	{
		[Required(ErrorMessage = "Ders Kodu alanı boş bırakılamaz.")]
		[Display(Name = "Ders Kodu")]
		public string Code { get; set; }

		[Required(ErrorMessage = "Ders ismi alanı boş bırakılamaz.")]
		[Display(Name = "Ders İsmi")]
		public string Name { get; set; }

		[AllowHtml]
		[Display(Name = "Açıklama")]
		public string Description { get; set; }

		[Display(Name = "Görünürlük Durumu")]
		public bool CanEveryoneJoin { get; set; }
		public Guid Id { get; set; }

		public string Heading => Code + " - " + Name;
	}
	
}