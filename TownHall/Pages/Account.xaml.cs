using System.Collections.ObjectModel;

namespace TownHall;

public class AccountViewModel
{
	public string UserName { get; set; } = "";
	public string Email { get; set; } = "";
	public ObservableCollection<Item> Listings { get; set; } = new();
}

public partial class Account : PageWithNavBar
{
	private readonly IUserService _userService;
	private readonly IItemService _itemService;

	public Account(IUserService userService, IItemService itemService)
	{
		InitializeComponent();
		_userService = userService;
		_itemService = itemService;

		LoadData();
	}

	protected override void OnAppearing()
	{
		base.OnAppearing();

		// Set minimum height of container to 60% of page height
		ListingsContainer.MinimumHeightRequest = this.Height * 0.6;

		// Show placeholder if there are no listings
		var listings = (BindingContext as AccountViewModel)?.Listings;
		if (listings == null || !listings.Any())
		{
			EmptyPlaceholder.IsVisible = true;
			ListingsCollectionView.IsVisible = false;
		}
		else
		{
			EmptyPlaceholder.IsVisible = false;
			ListingsCollectionView.IsVisible = true;
		}
	}

	private void LoadData()
	{
		var listings = _itemService.GetAllItems(true);

		BindingContext = new AccountViewModel
		{
			UserName = GlobalCurrentUser.User.FirstName,
			Email = GlobalCurrentUser.User.Email,
			Listings = new ObservableCollection<Item>(listings)
		};
	}
}


