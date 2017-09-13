using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PLE444.Models
{
	public class EventReponse
	{
		public enum Response {Join, Decline };
		[Key]
		public Event Event { get; set; }
		[Key]
		public String UserId { get; set; }
		public Response Status { get; set; }
	}
}