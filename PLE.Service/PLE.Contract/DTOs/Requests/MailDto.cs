using System.Collections.Generic;

namespace PLE.Contract.DTOs.Requests
{
	public class MailDto
	{
		public static class Templates
		{
			public const string NewAssignment = "NewAssignment";
			public const string ConfirmEmail = "ConfirmEmail";
			public const string NewMessage = "NewMessage";
			public const string ResetPassword = "ResetPassword";
		}

		public string Title { get; set; }

		public string Template { get; set; }

		public object Model { get; set; }

		public List<string> To { get; set; }
		
		public List<string> Bcc { get; set; }
	}
}
