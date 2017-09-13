using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PLE.Contract.DTOs
{
	public class UserGrade
	{
		[Key]
		public int Id { get; set; }

		[ForeignKey("User")]
		public string UserId { get; set; }

		public virtual User User { get; set; }

		[ForeignKey("GradeType")]
		public int GradeTypeId { get; set; }

		public GradeType GradeType { get; set; }

		public float Grade { get; set; }
	}
}