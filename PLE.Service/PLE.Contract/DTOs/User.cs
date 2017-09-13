using System;
using PLE.Contract.Enums;

namespace PLE.Contract.DTOs
{
	public class User
	{
		public Guid Id { get; set; }

		public string FirstName { get; set; }

		public string LastName { get; set; }

		public string UserName { get; set; }

		public string Email { get; set; }

		public GenderType Gender { get; set; }

		public string ProfilePicture { get; set; }

		public string PhoneNo { get; set; }

		public string Vision { get; set; }

		public string Mission { get; set; }
		
		public string FullName()
		{
			return FirstName + " " + LastName;
		}

		public string UserPhoto()
		{
			return string.IsNullOrWhiteSpace(ProfilePicture) ? "~/Content/img/pp.jpg" : ProfilePicture;
		}
	}
}