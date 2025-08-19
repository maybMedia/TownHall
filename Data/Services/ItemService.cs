using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TownHall
{
	public class ItemService : IItemService
	{
		IUnitOfWork _unitOfWork;

		public ItemService(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		// methods
	}
}
