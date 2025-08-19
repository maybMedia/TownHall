using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TownHall
{
	public interface IUnitOfWork : IDisposable
	{
		// repository fields
		IItemRepository ItemRepository { get; }
		

		public void SaveChanges();
	}
}
