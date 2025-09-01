namespace TownHall
{
	public partial class App : Application
	{
		private readonly IUserService _userService;

		public App(IUserService userService)
		{
			InitializeComponent();
			_userService = userService;

			// Start with Login page
			MainPage = new Login(_userService);
		}
	}
}