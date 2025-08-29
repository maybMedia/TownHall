namespace TownHall;

public partial class Buy : PageWithNavBar
{
	private IItemService _itemService;

	public Buy(IItemService itemService)
	{
		InitializeComponent();

		_itemService = itemService;
	}

	private void OnSearchBarButtonPressed(object sender, EventArgs e)
	{
		var query = Search.Text;
		SearchResultLabel.Text = $"Search query: {query}";

		var items = _itemService.SearchForItems(query, false);
	}

	private async void OnGoToListingsClicked(object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync($"{nameof(Listings)}?itemId={_itemService.GetItemById(new Guid("9CB9C726-B0F9-4079-9C95-222C24CE3F0C")).Id}"); // hardcoded for now
	}
}