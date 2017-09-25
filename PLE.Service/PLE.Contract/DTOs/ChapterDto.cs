using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLE.Contract.DTOs
{
	public class ChapterDto
	{
		public Guid Id { get; set; }

		public virtual CourseDto Course { get; set; }

		public string Title { get; set; }

		public string Description { get; set; }

		public DateTime DateAdded { get; set; }

		public virtual ICollection<MaterialDto> Materials { get; set; }
	}
}
