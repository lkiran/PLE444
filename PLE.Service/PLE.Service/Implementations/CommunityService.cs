using AutoMapper;
using PLE.Contract.DTOs;
using PLE.Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PLE.Service.Implementations
{
    public class CommunityService
    {
        private readonly PleDbContext db = new PleDbContext();
        public List<CommunityDto> GetCommunityListByUser(string userId)
        {
            var userCommunities = db.UserCommunities.Where(uc => uc.UserId == userId && uc.IsActive);
            var communities = db.Communities.Where(c => c.OwnerId == userId && c.IsActive);
            var data = (from p in userCommunities select p.Community).Union(communities);
            var result = Mapper.Map<List<CommunityDto>>(data.ToList());
            return result;
        }
    }
}