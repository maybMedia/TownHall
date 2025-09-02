using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using TownHall.Core;

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
	IMessageService _messageService;
	IUserService _userService;

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

	public Buy(IItemService itemService, IMessageService messageService, IUserService userService)
	{
		InitializeComponent();

		_itemService = itemService;
		_messageService = messageService;
		_userService = userService;

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
				if (_messageService.GetMessagesByBuyerIdAndItemId(GlobalCurrentUser.User.Id, item.Id).Count < 1)
				{
					_messageService.SendMessage(new Message
					{
						Buyer = GlobalCurrentUser.User,
						BuyerId = GlobalCurrentUser.User.Id,
						Seller = _userService.GetUserById(item.SellerId),
						SellerId = item.SellerId,
						Item = item,
						ItemId = item.Id,
						Sender = GlobalCurrentUser.User,
						SenderId = GlobalCurrentUser.User.Id,
						Content = "Hello, I am interested in your item.",
						Timestamp = DateTime.UtcNow
					});
				}
				await Shell.Current.GoToAsync($"{nameof(Messages)}?messageId={item.Id}");
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