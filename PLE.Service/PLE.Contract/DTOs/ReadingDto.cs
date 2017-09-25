using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLE.Contract.DTOs
{
	public class ReadingDto
	{
		public int Id { get; set; }
		
		public UserDto User { get; set; }

		public DateTime Date { get; set; }
	}
}
