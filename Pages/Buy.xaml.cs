using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace TownHall;

public partial class Buy : PageWithNavBar, INotifyPropertyChanged
{
	// need this to make the UI reactive
	public event PropertyChangedEventHandler PropertyChanged;
	protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
	{
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}

	private IItemService _itemService;

	public ICommand CardTappedCommand { get; }

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

	public Buy(IItemService itemService)
	{
		InitializeComponent();

		_itemService = itemService;

		_displayedItems.CollectionChanged += (s, e) =>
		{
			OnPropertyChanged(nameof(NoItemsToDisplay));
		};

		CardTappedCommand = new Command<Item>(async item =>
		{
			if (item != null)
			{
				await Shell.Current.GoToAsync($"{nameof(Listings)}?itemId={item.Id}");
			}
		});

		BindingContext = this;
	}

	protected override void OnNavigatedTo(NavigatedToEventArgs args)
	{
		base.OnNavigatedTo(args);

		DisplayedItems.Clear();
		var items = _itemService.SearchForItems(SearchQuery, false);
		foreach (var item in items)
		{
			DisplayedItems.Add(item);
		}
	}

	private void OnSearchBarButtonPressed(object sender, EventArgs e)
	{
		DisplayedItems.Clear();
		var items = _itemService.SearchForItems(SearchQuery, false);
		foreach (var item in items)
		{
			DisplayedItems.Add(item);
		}
	}
}