namespace TownHall.Core
{
	public class UnitOfWork : IUnitOfWork
	{
		private readonly TownHallContext _context;

		public IItemRepository ItemRepository { get; }

		public IUserRepository UserRepository { get; }

		public UnitOfWork(TownHallContext context)
		{
			_context = context;
			ItemRepository = new ItemRepository(_context);
			UserRepository = new UserRepository(_context);
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
