using System.Reflection;
using System.Windows.Input;

namespace TownHall;

public partial class Buy : PageWithNavBar
{
	private IItemService _itemService;

	public ICommand CardTappedCommand { get; }

	public List<Item> DisplayedItems { get; set; }

	public Buy(IItemService itemService)
	{
		InitializeComponent();

		_itemService = itemService;

		CardTappedCommand = new Command<Item>(async listing =>
		{
			if (listing != null)
			{
				await Shell.Current.GoToAsync($"listingdetails?listingId={listing.Id}");
			}
		});
	}

	protected override void OnNavigatedTo(NavigatedToEventArgs args)
	{
		base.OnNavigatedTo(args);

		ItemCollectionView.ItemsSource = _itemService.GetAllItems(false);
	}

	private void OnSearchBarButtonPressed(object sender, EventArgs e)
	{
		var items = _itemService.SearchForItems(Search.Text, false);
	}

	private async void OnGoToListingsClicked(object sender, EventArgs e)
	{
		var item = _itemService.GetItemById(new Guid("08275136-F858-4737-B531-1304E9B360F8")); // hardcoded for now
		await Shell.Current.GoToAsync($"{nameof(Listings)}?itemId={item.Id}"); 
	}
}