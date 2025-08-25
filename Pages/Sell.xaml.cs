namespace TownHall;

public partial class Sell : PageWithNavBar
{
	private IItemService _itemService;

	public Sell(IItemService itemService)
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
}