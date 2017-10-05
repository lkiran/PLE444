using PLE.Contract.DTOs;
using PLE.Website.Service.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace PLE.Website.Service
{
    public class CommunityService : IBaseService
    {
        public string Token { get; set; }

        private readonly PleClient _client;

        public CommunityService(string token)
        {
            _client = new PleClient(token);
        }

        public List<CommunityDto> GetCommunityListByUser([Optional]string userId)
        {
            var result = _client.Get<List<CommunityDto>>($"api/community/ListByUser/{userId}");
            if (result == null)
                result = new List<CommunityDto>();
            return result;
        }
    }
}
