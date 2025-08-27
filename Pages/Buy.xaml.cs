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

		var items = _itemService.SearchForItems(query);
	}

	private async void OnGoToListingsClicked(object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync(nameof(Listings));
	}
}