using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TownHall
{
	public interface IItemService
	{
		public List<Item> SearchForItems(string query);
	}
}
