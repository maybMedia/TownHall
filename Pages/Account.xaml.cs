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

	private void LoadData()
	{
		var currentUser = _userService.LoggedInUser;
		if (currentUser == null) return;

		// Ensure query is executed
		//var listings = _itemService.GetItemsByUser(currentUser.Id).ToList();

		BindingContext = new AccountViewModel
		{
			UserName = currentUser.FirstName,
			Email = currentUser.Email,
			//Listings = new ObservableCollection<Item>(listings)
		};
	}
}


