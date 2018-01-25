﻿using System;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using PLE.Contract.DTOs;
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
		public async Task<IHttpActionResult> GetUser(string userId="") {
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
	}
}
