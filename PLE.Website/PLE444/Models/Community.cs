﻿using System;
using System.Web.Mvc;
using System.ComponentModel;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PLE444.Models
{
	public class Community
	{
		public Community() {
			Id = Guid.NewGuid();
			IsActive = true;
		}

		[Key]
		[DisplayName("ID")]
		public Guid Id { get; set; }

		[DisplayName("İsim")]
		public string Name { get; set; }

		[AllowHtml]
		[DisplayName("Açıklama")]
		public string Description { get; set; }

		[DisplayName("Açılma Tarihi")]
		public DateTime DateCreated { get; set; }

		[DisplayName("Aktif/Kapalı")]
		public bool IsActive { get; set; }

		[DisplayName("Herkes/Onay")]
		public bool IsOpen { get; set; }

		[DisplayName("Gizli/Görünür")]
		public bool IsHiden { get; set; }

		[ForeignKey("Owner")]
		[DisplayName("Oluşturan")]
		public string OwnerId { get; set; }

		[DisplayName("Oluşturan")]
		public ApplicationUser Owner { get; set; }

		public virtual ICollection<Discussion> Discussions { get; set; }
	}
}