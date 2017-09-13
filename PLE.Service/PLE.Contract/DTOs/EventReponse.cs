using System.ComponentModel.DataAnnotations;
namespace PLE.Contract.DTOs
{
	public class EventReponse
	{
		public enum Response {Join, Decline };

		[Key]
		public Event Event { get; set; }

		[Key]
		public string UserId { get; set; }

		public Response Status { get; set; }
	}
}