using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PLE444.Models
{
	public class Community
	{
		public Community()
		{
			ID = Guid.NewGuid();
		}

		[Required]
		public Guid ID { get; private set; }
		public String Name { get; set; }
		public String Description { get; set; }
        public bool isOpen { get; set; }
        public bool isHiden { get; set; }
        public bool canCreateEvents { get; set; }
        public string GroupPhoto { get; set; }
        public string AdminId { get; set; }
        public virtual ICollection<Discussion> Discussion { get; set; }

    }
}