namespace TownHall;

public partial class Login : ContentPage
{
	private IUserService _userService;

	public Login(IUserService userService)
	{
		InitializeComponent();

		_userService = userService;

		if (Preferences.ContainsKey("SavedEmail"))
		{
			EmailEntry.Text = Preferences.Get("SavedEmail", string.Empty);
			RememberMeCheckBox.IsChecked = true;
		}
	}

	private async void OnForgotPasswordClicked(object? sender, EventArgs eventArgs)
	{
		await DisplayAlert("Forgot Password", "Good luck!", "OK");
	}

	private async void OnLoginClicked(object sender, EventArgs e)
	{
		bool isAuthenticated = _userService.Login(EmailEntry.Text, PasswordEntry.Text);

		if (isAuthenticated)
		{
			if (RememberMeCheckBox.IsChecked)
			{
				Preferences.Set("SavedEmail", EmailEntry.Text);
			}
			else
			{
				Preferences.Remove("SavedEmail");
			}

			// Swap out login page with the main shell
			Application.Current.MainPage = new AppShell();
		}
		else
		{
			await DisplayAlert("Error", "Invalid login", "OK");
		}
	}


	private void OnSignUpClicked(object? sender, EventArgs e)
	{
		Application.Current.MainPage = new Signup(_userService);
	}
}