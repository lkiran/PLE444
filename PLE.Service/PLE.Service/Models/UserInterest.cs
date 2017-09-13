using System.ComponentModel.DataAnnotations;

namespace PLE.Service.Models
{
	public class UserInterest
	{
		[Key]
		public string UserId { get; set; }
		[Key]
		public Course Course { get; set; }
	}
}