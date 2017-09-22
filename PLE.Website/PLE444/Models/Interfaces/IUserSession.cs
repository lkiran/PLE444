namespace PLE444.Models.Interfaces
{public interface IUserSession
	{
		string Email { get; }

		string BearerToken { get; }
	}
}