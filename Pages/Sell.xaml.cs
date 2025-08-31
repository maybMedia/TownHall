using System.Windows.Input;

namespace TownHall;

public partial class Sell : PageWithNavBar
{
	private IItemService _itemService;
	private IUserService _userService;

	public ICommand CardTappedCommand { get; }

	public List<Item> DisplayedItems { get; set; } = new List<Item>();

	public bool HasNoListings => DisplayedItems.Count == 0;

	public Sell(IItemService itemService, IUserService userService)
	{
		InitializeComponent();

		_itemService = itemService;
		_userService = userService;

		CardTappedCommand = new Command<Item>(async item =>
		{
			if (item != null)
			{
				await Shell.Current.GoToAsync($"{nameof(Listings)}?itemId={item.Id}");
			}
		});
	}

	protected override void OnNavigatedTo(NavigatedToEventArgs args)
	{
		base.OnNavigatedTo(args);

		DisplayedItems = _itemService.GetAllItems(true);
		ItemCollectionView.ItemsSource = DisplayedItems;
	}

	private void OnSearchBarButtonPressed(object sender, EventArgs e)
	{
		var items = _itemService.SearchForItems(Search.Text, true);
	}

	private void OnCreateNewClicked(object sender, EventArgs e)
	{
		Shell.Current.GoToAsync($"{nameof(Listings)}?itemId={Guid.Empty}"); // empty guid indicates new item
	}
}