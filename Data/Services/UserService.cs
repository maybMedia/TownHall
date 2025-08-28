namespace TownHall
{
	public class UserService : IUserService
	{
		public User? LoggedInUser;

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
				LoggedInUser = user;
				return true;
			}
			return false;
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
