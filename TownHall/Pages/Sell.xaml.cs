using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using TownHall.Core;

namespace TownHall;

public partial class Sell : PageWithNavBar, INotifyPropertyChanged
{
	// need this to make the UI reactive
	public event PropertyChangedEventHandler PropertyChanged;
	protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
	{
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}

	private IItemService _itemService;

	public ICommand ViewCommand { get; }
	public ICommand MessageCommand { get; }

	public string SearchQuery => Search.Text ?? string.Empty;

	private ObservableCollection<Item> _displayedItems = new ObservableCollection<Item>();
	public ObservableCollection<Item> DisplayedItems
	{
		get => _displayedItems;
		set
		{
			if (_displayedItems != value)
			{
				_displayedItems = value;
				OnPropertyChanged(nameof(DisplayedItems));
			}
		}
	}

	public bool NoItemsToDisplay => DisplayedItems.Count == 0;

	public Sell(IItemService itemService)
	{
		InitializeComponent();

		_itemService = itemService;

		_displayedItems.CollectionChanged += (s, e) =>
		{
			OnPropertyChanged(nameof(NoItemsToDisplay));
		};

		ViewCommand = new Command<Item>(async item =>
		{
			if (item != null)
			{
				await Shell.Current.GoToAsync($"{nameof(Listings)}?itemId={item.Id}");
			}
		});
		MessageCommand = new Command<Item>(async item =>
		{
			if (item != null)
			{
				await Shell.Current.GoToAsync($"{nameof(Messages)}?messageId={item.Id}");
			}
		});

		BindingContext = this;
	}

	protected override void OnNavigatedTo(NavigatedToEventArgs args)
	{
		base.OnNavigatedTo(args);

		DisplayedItems.Clear();
		var items = _itemService.SearchForItems(SearchQuery, true);
		foreach (var item in items)
		{
			DisplayedItems.Add(item);
		}
	}

	private void OnSearchBarButtonPressed(object sender, EventArgs e)
	{
		DisplayedItems.Clear();
		var items = _itemService.SearchForItems(SearchQuery, true);
		foreach (var item in items)
		{
			DisplayedItems.Add(item);
		}
	}

	private void OnCreateNewClicked(object sender, EventArgs e)
	{
		Shell.Current.GoToAsync($"{nameof(Listings)}?itemId={Guid.Empty}"); // empty guid indicates new item
	}
}