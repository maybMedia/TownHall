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
		var item = _itemService.GetItemById(new Guid("08275136-F858-4737-B531-1304E9B360F8")); // hardcoded for now
		await Shell.Current.GoToAsync($"{nameof(Listings)}?itemId={item.Id}"); 
	}
}