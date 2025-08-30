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
