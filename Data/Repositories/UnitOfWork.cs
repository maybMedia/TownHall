using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TownHall
{
	public class UnitOfWork : IUnitOfWork
	{
		private readonly TownHallContext _context;

		public IItemRepository ItemRepository { get; }

		public UnitOfWork(TownHallContext context)
		{
			_context = context;
			ItemRepository = new ItemRepository(_context);
		}

		public void Dispose()
		{
			_context.Dispose();
		}

		public void SaveChanges()
		{
			_context.SaveChanges();
		}
	}
}
