using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using PLE.Service.Models;

namespace PLE.Service.Controllers
{
    [RoutePrefix("api/accounts")]
    public class AccountsController : BaseApiController
    {
        private PleDbContext _db = new PleDbContext();

  
        [Route("users")]
        public IHttpActionResult GetUsers()
        {
            return Ok(_db.Users.ToList());
        }

        [Route("exampleException")]
        public IHttpActionResult GetExampleException()
        {
            return InternalServerError(new Exception("This an exception result (Levent Kıran)"));
        }

        [Route("user/{id:guid}", Name = "GetUserById")]
        public async Task<IHttpActionResult> GetUser(string id)
        {
            var user = await AppUserManager.FindByIdAsync(id);

            if (user != null)
            {
                return Ok(TheModelFactory.Create(user));
            }

            return NotFound();
        }

        [Route("user/{username}")]
        public async Task<IHttpActionResult> GetUserByName(string username)
        {
            var user = await this.AppUserManager.FindByNameAsync(username);

            if (user != null)
            {
                return Ok(this.TheModelFactory.Create(user));
            }

            return NotFound();

        }
    }
}
