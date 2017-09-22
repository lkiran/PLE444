using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLE.Contract.DTOs
{
	public class TokenDto
	{
		public string access_token { get; set; }

		public int expires_in { get; set; }

		public string token_type { get; set; }
	}
}
