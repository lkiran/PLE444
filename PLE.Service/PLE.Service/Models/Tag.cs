using System.ComponentModel.DataAnnotations;

namespace PLE.Service.Models
{
	public class Tag
	{
		[Key]
		public string Name { get; set; }
	}
}