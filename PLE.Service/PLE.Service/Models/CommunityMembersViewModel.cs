using System.Collections.Generic;

namespace PLE.Service.Models
{
	public class CommunityMembersViewModel
	{
		public Community CommunityInfo { get; set; }

		public List<UserViewModel> Users { get; set; }
	}
}