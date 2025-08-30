namespace TownHall
{
	public class UserService : IUserService
	{
		private IUnitOfWork _unitOfWork;

		public UserService(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public bool Login(string email, string password)
		{
			var user = _unitOfWork.UserRepository.ValidateCredentials(email, password);
			if (user != null)
			{
				GlobalCurrentUser.User = user;
				return true;
			}
			return false;
		}

		public User CreateUser(string email, string password, string firstName, string lastName, string phone, string address)
		{
			var newUser = new User
			{
				Id = Guid.NewGuid(),
				Email = email,
				Password = password,
				FirstName = firstName,
				LastName = lastName,
				Phone = phone,
				Address = address
			};
			var addedUser = _unitOfWork.UserRepository.Add(newUser);
			_unitOfWork.SaveChanges();
			return addedUser;
		}

		public List<User> GetUsers()
		{
			return _unitOfWork.UserRepository.GetUsers();
		}

		public User? GetUserById(Guid id)
		{
			return _unitOfWork.UserRepository.GetById(id);
		}
	}
}
