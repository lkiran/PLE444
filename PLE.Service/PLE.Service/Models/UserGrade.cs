using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PLE.Service.Models
{
	public class UserGrade
	{
		[Key]
		public int Id { get; set; }

		[ForeignKey("User")]
		public string UserId { get; set; }

		public virtual ApplicationUser User { get; set; }

		[ForeignKey("GradeType")]
		public int GradeTypeId { get; set; }

		public GradeType GradeType { get; set; }

		public float Grade { get; set; }
	}
}