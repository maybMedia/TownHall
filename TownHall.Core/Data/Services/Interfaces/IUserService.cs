namespace TownHall.Core
{
	public interface IUserService
	{
		public bool Login(string email, string password);

		public List<User> GetUsers();

		public User? GetUserById(Guid id);

		public User CreateUser(string email, string password, string firstName, string lastName, string phone, string address);
	}
}
