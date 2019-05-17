using System;
using System.Web;
using AutoMapper;
using System.Linq;
using PLE.Contract.DTOs;
using PLE.Service.Models;
using PLE.Service.App_Start;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using PLE.Contract.DTOs.Responses;
using Microsoft.AspNet.Identity.Owin;

namespace PLE.Service.Implementations {
	public class UserService {
		private readonly PleDbContext _db = new PleDbContext();
		private ApplicationUserManager _userManager;

		public ApplicationUserManager UserManager {
			get => _userManager ?? HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
			private set => _userManager = value;
		}


		public ApplicationUser Get(string userId) {
			return _db.Users.Find(userId);
		}


		public ApplicationUser GetByEmail(string email) {
			return _db.Users.FirstOrDefault(u => u.Email == email);
		}

		public async Task<RegisterUserResponseDto> SaveAsync(UserDto user) {
			var applicationUser = Mapper.Map<ApplicationUser>(user);
			applicationUser.Id = Guid.NewGuid().ToString();
			applicationUser.Role = ApplicationUser.RoleType.User;

			var result = await UserManager.CreateAsync(applicationUser, user.Password);

			return new RegisterUserResponseDto {
				UserId = applicationUser.Id,
				Errors = result.Errors.ToList(),
				Status = result.Succeeded
			};
		}

		#region E-Mail Verification
		public async Task<string> GetEmailVerificationCode(Guid userId) {
			var code = await UserManager.GenerateEmailConfirmationTokenAsync(userId.ToString());

			return code;
		}

		public async Task<IdentityResult> VerifyEmail(Guid userId, string code) {
			code = code.Replace(" ", "+");
			var result = await UserManager.ConfirmEmailAsync(userId.ToString(), code);

			return result;
		}

		public async Task<bool> IsEmailConfirmed(Guid userId) {
			var result = await UserManager.IsEmailConfirmedAsync(userId.ToString());

			return result;
		}
		#endregion

		#region Forgot Password
		public async Task<string> GetPasswordResetCode(Guid userId) {
			var code = await UserManager.GeneratePasswordResetTokenAsync(userId.ToString());

			return code;
		}

		public async Task<IdentityResult> ResetPassword(Guid userId, string newPassword, string code) {
			code = code.Replace(" ", "+");
			var result = await UserManager.ResetPasswordAsync(userId.ToString(), code, newPassword);

			return result;
		}
		#endregion
	}
}