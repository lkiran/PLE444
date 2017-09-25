using PLE.Contract.Enums;

namespace PLE.Contract.DTOs
{
	public class UserDto
	{
		public string Id { get; set; }

		public TokenDto Token { get; set; }

		public string FirstName { get; set; }

		public string LastName { get; set; }

		public string UserName { get; set; }

		public string Email { get; set; }

		public GenderType Gender { get; set; }

		public string ProfilePicture { get; set; }

		public string PhoneNo { get; set; }

		public string Vision { get; set; }

		public string Mission { get; set; }

		public string FullName() => FirstName + " " + LastName;

		public string UserPhoto() => string.IsNullOrWhiteSpace(ProfilePicture) ? "~/Content/img/pp.jpg" : ProfilePicture;
	}
}