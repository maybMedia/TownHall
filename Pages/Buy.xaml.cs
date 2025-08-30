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
		var item = _itemService.GetItemById(new Guid("CB43028B-F1E9-4B64-B590-DF655D06A641")); // hardcoded for now
		await Shell.Current.GoToAsync($"{nameof(Listings)}?itemId={item.Id}"); 
	}
}