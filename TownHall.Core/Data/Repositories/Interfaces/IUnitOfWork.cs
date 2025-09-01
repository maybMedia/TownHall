namespace TownHall.Core
{
	public interface IUnitOfWork : IDisposable
	{
		// repository fields
		IItemRepository ItemRepository { get; }

		IUserRepository UserRepository { get; }

		public void SaveChanges();
	}
}
