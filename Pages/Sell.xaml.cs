namespace TownHall;

public partial class Sell : PageWithNavBar
{
	public Sell() : base("Sell")
	{
		InitializeComponent();
	}

	private async void GoToMessages(object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync(nameof(Messages));
	}

	private async void GoToBuy(object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync(nameof(Buy));
	}

	private async void GoToSell(object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync(nameof(Sell));
	}

	private async void GoToAccount(object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync(nameof(Account));
	}
}