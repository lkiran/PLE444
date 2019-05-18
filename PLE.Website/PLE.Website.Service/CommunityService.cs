﻿using PLE.Contract.DTOs;
using PLE.Website.Service.Core;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace PLE.Website.Service
{
	public class CommunityService: BaseService
	{

		public List<CommunityDto> GetCommunityListByUser([Optional]string userId) {
			var result = Client.Get<List<CommunityDto>>($"api/community/ListByUser/{userId}");

			if (result == null)
				result = new List<CommunityDto>();

			return result;
		}
	}
}
