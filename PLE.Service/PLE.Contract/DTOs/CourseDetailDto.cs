using System.Collections.Generic;

namespace PLE.Contract.DTOs
{
	public class CourseDetailDto: CourseDto
	{
		public int MemberCount { get; set; }

		public virtual ICollection<CourseDto> Terms { get; set; }

		public virtual ICollection<TimelineEntryDto> Timeline { get; set; }
	}
}