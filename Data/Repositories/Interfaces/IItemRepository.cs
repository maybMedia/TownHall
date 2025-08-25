using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TownHall
{
	public interface IItemRepository
	{
		public List<Item> GetByName(string name);
	}
}
