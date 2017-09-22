using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using PLE.Contract.DTOs.Requests;
using PLE.Service.Models;

namespace PLE.Service.Controllers
{
	[RoutePrefix("api/accounts")]
	public class AccountsController : BaseApiController
	{
		private PleDbContext _db = new PleDbContext();

		[HttpGet]
		[Route("users")]
		public IHttpActionResult GetUsers() {
			return Ok(_db.Users.ToList());
		}

		[HttpGet]
		[Route("exampleException")]
		public IHttpActionResult GetExampleException() {
			return InternalServerError(new Exception("This an exception result (Levent Kıran)"));
		}



		[HttpPost]
		[Route("GetUser")]
		public async Task<IHttpActionResult> GetUser(GetUserRequest request)
		{
			var user = _db.Users.FirstOrDefault(u => u.UserName == request.UserName);

			if (user != null) {
				return Ok(user);
			}

			return NotFound();

		}
	}
}
