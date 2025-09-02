namespace TownHall.Core
{
	public class UserRepository : Repository<User>, IUserRepository
	{
		public UserRepository(TownHallContext context) : base(context)
		{
		}

		public TownHallContext TownHallContext
		{
			get { return _context as TownHallContext; }
		}

		public User? ValidateCredentials(string email, string password)
		{
			if (_context.Users.Any(u => u.Email == email && u.Password == password))
			{
				return _context.Users.FirstOrDefault(u => u.Email == email && u.Password == password);
			}
			return null;
		}

		public User? GetById(Guid id)
		{
			return _context.Users.FirstOrDefault(u => u.Id == id);
		}

		public List<User> GetUsers()
		{
			return _context.Users.ToList();
		}

		public User Add(User user)
		{
			_context.Users.Add(user);
			return user;
		}
	}
}
