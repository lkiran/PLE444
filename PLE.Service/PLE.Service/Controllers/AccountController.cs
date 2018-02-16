using System;
using System.Web.Http;
using PLE.Contract.DTOs;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using PLE.Service.Implementations;

namespace PLE.Service.Controllers
{
	[RoutePrefix("Api/Accounts")]
	public class AccountsController : BaseApiController
	{
		private UserService _userService;

		public AccountsController() {
			_userService = new UserService();
		}


		[HttpGet]
		[Route("User")]
		public async Task<IHttpActionResult> GetUser(string userId = "") {
			if (string.IsNullOrWhiteSpace(userId)) {
				if (User.Identity.GetUserId() == null)
					return Unauthorized();
				userId = User.Identity.GetUserId();
			}

			var user = _userService.Get(userId);

			if (user != null)
				return Ok(user);

			return NotFound();
		}


		[HttpPost]
		[Route("User")]
		public async Task<IHttpActionResult> Register(UserDto user) {
			try {
				var result = await _userService.SaveAsync(user);
				return Ok(result);
			}
			catch (Exception e) {
				return InternalServerError(e);
			}
		}

		#region E-Mail Validation
		[HttpGet]
		[Route("EmailVerificationCode")]
		public async Task<IHttpActionResult> EmailVerificationCode(Guid userId) {
			try {
				if (userId == Guid.Empty) {
					ModelState.AddModelError("", "User Id is required");
					return BadRequest(ModelState);
				}

				var token = await _userService.GetEmailVerificationCode(userId);

				return Ok(token);
			}
			catch (Exception e) {
				return InternalServerError(e);
			}
		}


		[HttpGet]
		[Route("ConfirmEmail", Name = "ConfirmEmailRoute")]
		public async Task<IHttpActionResult> ConfirmEmail(Guid userId, string code) {
			try {
				if (userId == Guid.Empty || string.IsNullOrWhiteSpace(code)) {
					ModelState.AddModelError("", "User Id and Code are required");
					return BadRequest(ModelState);
				}


				var result = await _userService.VerifyEmail(userId, code);

				return result.Succeeded ? Ok() : GetErrorResult(result);
			}
			catch (Exception e) {
				return InternalServerError(e);
			}
		}


		[HttpGet]
		[Route("IsEmailConfirmed", Name = "IsEmailConfirmedRoute")]
		public async Task<IHttpActionResult> IsEmailConfirmed(Guid userId) {
			try {
				if (userId == Guid.Empty) {
					ModelState.AddModelError("", "User Id is required");
					return BadRequest(ModelState);
				}

				var result = await _userService.IsEmailConfirmed(userId);

				return Ok(result);
			}
			catch (Exception e) {
				return InternalServerError(e);
			}
		}
		#endregion

		#region Forgot Password
		[HttpGet]
		[Route("PasswordResetCode")]
		public async Task<IHttpActionResult> PasswordResetCode(Guid userId) {
			try {
				if (userId == Guid.Empty) {
					ModelState.AddModelError("", "User Id is required");
					return BadRequest(ModelState);
				}

				var token = await _userService.GetPasswordResetCode(userId);

				return Ok(token);
			}
			catch (Exception e) {
				return InternalServerError(e);
			}
		}


		[HttpGet]
		[Route("PasswordReset", Name = "PasswordResetRoute")]
		public async Task<IHttpActionResult> PasswordReset(Guid userId, string newPassword, string code) {
			try {
				if (userId == Guid.Empty || string.IsNullOrWhiteSpace(code)) {
					ModelState.AddModelError("", "User Id and Code are required");
					return BadRequest(ModelState);
				}

				var result = await _userService.ResetPassword(userId, newPassword, code);

				return result.Succeeded ? Ok() : GetErrorResult(result);
			}
			catch (Exception e) {
				return InternalServerError(e);
			}
		} 
		#endregion
	}
}
