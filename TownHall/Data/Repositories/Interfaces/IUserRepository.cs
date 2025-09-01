namespace TownHall
{
	public interface IUserRepository
	{
		public User? ValidateCredentials(string email, string password);
		public User? GetById(Guid id);
		public List<User> GetUsers();
		User Add(User newUser);
	}
}
