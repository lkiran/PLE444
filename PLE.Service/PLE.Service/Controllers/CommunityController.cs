using Microsoft.AspNet.Identity;
using PLE.Service.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace PLE.Service.Controllers
{
    [RoutePrefix("Api/Community")]
    public class CommunityController:BaseApiController
    {
        private CommunityService _communityService;

        public CommunityController()
        {
            _communityService = new CommunityService();
        }

        [HttpGet]
        [Route("ListByUser/{userId?}")]
        public IHttpActionResult GetByUser(string userId = null)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                if (User.Identity.GetUserId() == null) //current user a erişiyor
                    return Unauthorized();
                userId = User.Identity.GetUserId();
            }

            var result = _communityService.GetCommunityListByUser(userId);

            return Ok(result);
        }

    }
}