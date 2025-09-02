using System.Collections.ObjectModel;
using System.Globalization;
using TownHall.Core;

namespace TownHall;

[QueryProperty(nameof(ItemId), "itemId")]
[QueryProperty(nameof(BuyerId), "buyerId")]
public partial class Messages : PageWithNavBar
{
	private readonly MessagesViewModel _viewModel;

	private IMessageService _messageService;
	private IItemService _itemService;
	private IUserService _userService;

	public string ItemId { get; set; }
	private Guid? itemId => string.IsNullOrWhiteSpace(ItemId) ? null : Guid.Parse(ItemId);

	public string BuyerId { get; set; }
	private Guid? buyerId => string.IsNullOrWhiteSpace(BuyerId) ? null : Guid.Parse(BuyerId);

	public Messages(IMessageService messageService, IUserService userService, IItemService itemService)
	{
		InitializeComponent();

		// resolve IMessageService + current user from DI/session
		_messageService = messageService;
		_itemService = itemService;
		_userService = userService;

		var currentUserId = GlobalCurrentUser.User.Id;

		_viewModel = new MessagesViewModel(_messageService, _itemService, _userService, currentUserId);
		BindingContext = _viewModel;
	}

	protected override async void OnAppearing()
	{
		base.OnAppearing();

		var grid = (Grid)Content;

		if (itemId.HasValue && buyerId.HasValue)
		{
			// Replace "Recent Conversations" with "Chat"
			ConversationCountLabel.Text = "Chat";

			// Hide the conversations list
			ConversationsCollectionView.IsVisible = false;

			// Add ChatView
			var chatView = new ChatView(itemId.Value, buyerId.Value, _messageService, _userService, _itemService);
			Grid.SetRow(chatView, 1);
			grid.Children.Add(chatView);
		}
		else
		{
			// Reset label if coming back to conversations
			ConversationCountLabel.Text = "Recent Conversations";

			ConversationsCollectionView.IsVisible = true;
			await _viewModel.LoadConversations();
		}
	}




	private async void OnConversationSelected(object sender, SelectionChangedEventArgs e)
	{
		if (e.CurrentSelection.FirstOrDefault() is ConversationViewModel conversation)
		{
			Console.WriteLine($"Navigating to chat: ItemId={conversation.ItemId}, BuyerId={conversation.BuyerId}");

			// Navigate to the same Messages page but pass ItemId and BuyerId as query parameters
			var route = $"Messages?itemId={conversation.ItemId}&buyerId={conversation.BuyerId}";
			await Shell.Current.GoToAsync(route);
		}

		// Deselect the item
		((CollectionView)sender).SelectedItem = null;
	}
}

public class MessagesViewModel : BindableObject
{
	private readonly IMessageService _messageService;
	private readonly IItemService _itemService;
	private readonly IUserService _userService;

	private readonly Guid _currentUserId; // you’ll need to inject/resolve this from auth/session

	private bool _isLoading;
	public bool IsLoading
	{
		get => _isLoading;
		set { _isLoading = value; OnPropertyChanged(); }
	}

	public MessagesViewModel(IMessageService messageService, IItemService itemService, IUserService userService, Guid currentUserId)
	{
		_messageService = messageService;
		_itemService = itemService;
		_userService = userService;
		_currentUserId = currentUserId;
	}

	private ObservableCollection<ConversationViewModel> _conversations = new();
	public ObservableCollection<ConversationViewModel> Conversations
	{
		get => _conversations;
		set { _conversations = value; OnPropertyChanged(); }
	}


	public async Task LoadConversations()
	{
		IsLoading = true;
		List<Conversation> conversations;

		conversations = _messageService.getUserConversations(_currentUserId);

		// Map to ConversationViewModel
		var mapped = conversations.Select(c =>
		{
			var lastMessage = c.Messages.OrderByDescending(m => m.Timestamp).FirstOrDefault();
			return new ConversationViewModel
			{
				ItemId = c.ItemId,
				BuyerId = c.BuyerId,
				SellerId = c.SellerId,

				DisplayName = c.BuyerId == _currentUserId ? _userService.GetUserById(c.SellerId).FirstName : _userService.GetUserById(c.BuyerId).FirstName, // TODO: lookup user
				LastMessagePreview = lastMessage?.Content ?? "",
				LastMessageTime = lastMessage?.Timestamp.ToShortTimeString() ?? "",
				UserInitials = c.BuyerId == _currentUserId ? _userService.GetUserById(c.SellerId).FirstName[0].ToString() + _userService.GetUserById(c.SellerId).LastName[0] : _userService.GetUserById(c.BuyerId).FirstName[0].ToString() + _userService.GetUserById(c.BuyerId).LastName[0],
				PictureData = _itemService.GetItemById(c.ItemId).ImageData,
				ShowUnreadIndicator = lastMessage != null && lastMessage.SenderId != _currentUserId
			};
		});

		Conversations = new ObservableCollection<ConversationViewModel>(mapped);
		IsLoading = false;
	}

}

public class ConversationViewModel
{
	public Guid ItemId { get; set; }
	public Guid BuyerId { get; set; }
	public Guid SellerId { get; set; }

	public string DisplayName { get; set; }
	public string LastMessagePreview { get; set; }
	public string LastMessageTime { get; set; }
	public string UserInitials { get; set; }
	public byte[] PictureData { get; set; }

	public bool HasItemPicture => PictureData != null && PictureData.Length > 0;

	public ImageSource ItemPictureUrl =>
		PictureData != null && PictureData.Length > 0
			? ImageSource.FromStream(() => new MemoryStream(PictureData))
			: null;

	public bool ShowUnreadIndicator { get; set; }
}

public class ChatView : ContentView
{
	private readonly Guid _itemId;
	private readonly Guid _buyerId;
	private readonly IMessageService _messageService;
	private readonly IUserService _userService;
	private readonly IItemService _itemService;

	private ObservableCollection<Message> _messages = new();
	private CollectionView _messagesList;
	private Entry _inputEntry;
	private Button _sendButton;

	public ChatView(Guid itemId, Guid buyerId, IMessageService messageService, IUserService userService, IItemService itemService)
	{
		_itemId = itemId;
		_buyerId = buyerId;
		_messageService = messageService;
		_userService = userService;
		_itemService = itemService;

		BuildUI();
		LoadMessages();
	}

	private void BuildUI()
	{
		_messagesList = new CollectionView
		{
			ItemsSource = _messages,
			ItemTemplate = new DataTemplate(() =>
			{
				var grid = new Grid
				{
					ColumnDefinitions =
				{
					new ColumnDefinition { Width = GridLength.Star },
					new ColumnDefinition { Width = GridLength.Star }
				},
					Margin = new Thickness(10, 2) // Increase left/right margin
				};

				var frame = new Frame
				{
					Padding = 10,
					CornerRadius = 10,
					HasShadow = false,
				};

				var label = new Label
				{
					LineBreakMode = LineBreakMode.WordWrap,
					FontSize = 14,
				};
				label.SetBinding(Label.TextProperty, "Content");

				frame.Content = label;

				frame.BindingContextChanged += (s, e) =>
				{
					if (frame.BindingContext is Message msg)
					{
						bool isCurrentUser = msg.SenderId == GlobalCurrentUser.User.Id;

						Grid.SetColumn(frame, isCurrentUser ? 1 : 0);
						frame.HorizontalOptions = isCurrentUser ? LayoutOptions.End : LayoutOptions.Start;

						// Add extra spacing from the edge
						frame.Margin = new Thickness(isCurrentUser ? 0 : 10, 2, isCurrentUser ? 10 : 0, 2);

						frame.BackgroundColor = isCurrentUser ? Colors.LightBlue : Colors.LightGray;
						label.TextColor = isCurrentUser ? Colors.White : Colors.Black;
					}
				};

				grid.Children.Add(frame);
				return grid;
			})
		};

		_inputEntry = new Entry
		{
			Placeholder = "Type a message...",
			HorizontalOptions = LayoutOptions.FillAndExpand
		};

		_sendButton = new Button
		{
			Text = "Send",
			WidthRequest = 80
		};
		_sendButton.Clicked += OnSendClicked;

		var inputLayout = new Grid
		{
			ColumnDefinitions =
		{
			new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
			new ColumnDefinition { Width = GridLength.Auto }
		},
			Padding = new Thickness(10, 5)
		};
		inputLayout.Add(_inputEntry, 0, 0);
		inputLayout.Add(_sendButton, 1, 0);

		// Wrap the messages list in a Grid with padding
		var mainGrid = new Grid
		{
			RowDefinitions =
		{
			new RowDefinition { Height = new GridLength(1, GridUnitType.Star) },
			new RowDefinition { Height = GridLength.Auto }
		},
			Padding = new Thickness(10) // This adds padding around the whole chat
		};
		mainGrid.Add(_messagesList, 0, 0);
		mainGrid.Add(inputLayout, 0, 1);

		Content = mainGrid;
	}


	private void LoadMessages()
	{
		var messages = _messageService.GetMessagesByBuyerIdAndItemId(_buyerId, _itemId);
		_messages.Clear();
		foreach (var msg in messages.OrderBy(m => m.Timestamp))
			_messages.Add(msg);

		// Scroll to bottom
		if (_messages.Count > 0)
			_messagesList.ScrollTo(_messages.Last(), position: ScrollToPosition.End);
	}

	private void OnSendClicked(object sender, EventArgs e)
	{
		if (!string.IsNullOrWhiteSpace(_inputEntry.Text))
		{
			var msg = new Message
			{
				BuyerId = _buyerId,
				SellerId = _itemService.GetItemById(_itemId).SellerId,
				ItemId = _itemId,
				SenderId = GlobalCurrentUser.User.Id,
				Content = _inputEntry.Text,
				Timestamp = DateTime.UtcNow
			};
			_messageService.SendMessage(msg);
			_messages.Add(msg);
			_inputEntry.Text = "";

			_messagesList.ScrollTo(msg, position: ScrollToPosition.End);
		}
	}
}

// Converter to align messages left/right depending on sender
public class MessageAlignmentConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (value is Guid senderId)
		{
			return senderId == GlobalCurrentUser.User.Id ? LayoutOptions.End : LayoutOptions.Start;
		}
		return LayoutOptions.Start;
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
}
