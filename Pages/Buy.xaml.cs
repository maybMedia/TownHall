using System.Reflection;
using System.Windows.Input;

namespace TownHall;

public partial class Buy : PageWithNavBar
{
	private IItemService _itemService;

	public ICommand CardTappedCommand { get; }

	public List<Item> DisplayedItems { get; set; } = new List<Item>();

	public Buy(IItemService itemService)
	{
		InitializeComponent();

		_itemService = itemService;

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

		DisplayedItems = _itemService.GetAllItems(false);
		ItemCollectionView.ItemsSource = DisplayedItems;
	}

	private void OnSearchBarButtonPressed(object sender, EventArgs e)
	{
		var items = _itemService.SearchForItems(Search.Text, false);
	}
}