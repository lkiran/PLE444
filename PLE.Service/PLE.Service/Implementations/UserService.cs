using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using AutoMapper;
using PLE.Contract.DTOs;
using PLE.Service.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using PLE.Contract.DTOs.Responses;
using PLE.Service.App_Start;

namespace PLE.Service.Implementations
{
	public class UserService
	{
		private readonly PleDbContext _db = new PleDbContext();
		private ApplicationUserManager _userManager;

		public ApplicationUserManager UserManager {
			get => _userManager ?? HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
			private set => _userManager = value;
		}

		public ApplicationUser Get(string userId) {
			return _db.Users.Find(userId);
		}

		public async Task<RegisterUserResponseDto> SaveAsync(UserDto user) {
			var applicationUser = Mapper.Map<ApplicationUser>(user);
			applicationUser.Id = Guid.NewGuid().ToString();
			applicationUser.EmailConfirmed = true;
			var result = await UserManager.CreateAsync(applicationUser, user.Password);
			
			return new RegisterUserResponseDto {
				UserId = applicationUser.Id,
				Errors = result.Errors.ToList(),
				Status = result.Succeeded
			};
		}
	}
}