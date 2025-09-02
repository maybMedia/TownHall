using System.Collections.ObjectModel;
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
		await _viewModel.LoadConversations();
	}

	private void OnConversationSelected(object? sender, SelectionChangedEventArgs e)
	{
		if (e.CurrentSelection.FirstOrDefault() is Conversation conversation)
		{
			// navigate to conversation detail page
			Shell.Current.GoToAsync(
				$"messages?itemId={conversation.ItemId}&buyerId={conversation.BuyerId}");
		}
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