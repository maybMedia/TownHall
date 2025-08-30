namespace TownHall;

public partial class Sell : PageWithNavBar
{
	private IItemService _itemService;
	private IUserService _userService;

	public Sell(IItemService itemService, IUserService userService)
	{
		InitializeComponent();

		_itemService = itemService;
		_userService = userService;
	}

	private void OnSearchBarButtonPressed(object sender, EventArgs e)
	{
		var query = Search.Text;
		SearchResultLabel.Text = $"Search query: {query}";

		var items = _itemService.SearchForItems(query, true);
	}

	private void OnCreateNewClicked(object sender, EventArgs e)
	{
		Shell.Current.GoToAsync($"{nameof(Listings)}?itemId={Guid.Empty}"); // empty guid indicates new item
	}
}