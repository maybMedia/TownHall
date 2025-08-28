namespace TownHall
{
	public interface IUserService
	{
		public bool Login(string email, string password);

		public List<User> GetUsers();

		public User? GetUserById(Guid id);
	}
}
