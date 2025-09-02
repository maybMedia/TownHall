namespace TownHall.Core
{
	public interface IUnitOfWork : IDisposable
	{
		// repository fields
		IItemRepository ItemRepository { get; }

		IUserRepository UserRepository { get; }

		IMessageRepository MessageRepository { get; }

		public void SaveChanges();
	}
}
