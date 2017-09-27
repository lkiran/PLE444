using System.Collections.Generic;

namespace PLE444.Models.Layout
{
	public class SidebarList<T>
	{
		public List<T> Items { get; set; }

		public string ActiveUserId { get; set; }
	}
}