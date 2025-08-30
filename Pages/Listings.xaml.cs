namespace TownHall;

[QueryProperty(nameof(ItemId), "itemId")]
public partial class Listings : PageWithNavBar
{
	private IItemService _itemService;
	private IUserService _userService;

	public string ItemId { get; set; }
	private Guid itemId => Guid.Parse(ItemId);

	public bool IsEditMode { get; set; } = false;
	public bool IsNewItem { get; set; } = true;	

	public Listings(IItemService itemService, IUserService userService)
	{
		InitializeComponent();

		_itemService = itemService;
		_userService = userService;
	}

	protected override void OnNavigatedTo(NavigatedToEventArgs args)
	{
		base.OnNavigatedTo(args);

		if (string.IsNullOrEmpty(ItemId))
		{
			throw new NullReferenceException();
		}

		if (itemId == Guid.Empty)
		{
			IsNewItem = true;
		}
		else
		{
			IsNewItem = false;

			var item = _itemService.GetItemById(itemId);
			PopulateEntryFields(item);

			IsEditMode = (item.SellerId == GlobalCurrentUser.User.Id);
		}
	}

	private void PopulateEntryFields(Item item)
	{
		PriceEntry.Text = item.Price.ToString();
		SummaryEntry.Text = item.Summary;
		NameEntry.Text = item.Name;
		DescriptionEntry.Text = item.Description;
		DateListedEntry.Text = item.ListedDate.ToString();
		LocationEntry.Text = item.Seller?.Address;
		SellerEntry.Text = item.Seller?.FullName;
	}

	private async void OnSaveClicked(object sender, EventArgs e)
	{
		var isValid = MapFieldsToItemObject(out var item);
		if (IsNewItem)
		{
			if (isValid)
			{
				_itemService.AddItem(item);

				await DisplayAlert("Created Successfully", "Your listing has been created.", "OK");
			}
			else 
			{
				await DisplayAlert("Error", "There was an error creating your listing. Please try again.", "OK");
			}
		}
		else
		{
			if (isValid)
			{
				_itemService.UpdateItem(item);
				await DisplayAlert("Saved Successfully", "Your changes have been saved.", "OK");
			}
			else
			{
				await DisplayAlert("Error", "There was an error saving your changes. Please try again.", "OK");
			}
		}
	}

	private bool MapFieldsToItemObject(out Item item)
	{
		item = _itemService.GetItemById(itemId) ?? new Item();

		// collect values from interface
		var priceText = PriceEntry.Text?.Trim();
		var summary = SummaryEntry.Text?.Trim();
		var name = NameEntry.Text?.Trim();
		var description = DescriptionEntry.Text?.Trim();

		// validate
		if (string.IsNullOrEmpty(priceText) ||
			string.IsNullOrEmpty(summary) ||
			string.IsNullOrEmpty(name) ||
			string.IsNullOrEmpty(description))
		{
			return false;
		}

		if (!decimal.TryParse(priceText, out decimal price)) return false;

		// now can map
		item.Id = IsNewItem ? Guid.NewGuid() : itemId;
		item.Name = name;
		item.Price = price;
		item.Summary = summary;
		item.Description = description;
		item.ListedDate = IsNewItem ? DateTime.Now : GetListedDate();
		item.IsAvailable = true; // will handle closing listings later
		item.SellerId = GlobalCurrentUser.User.Id;

		return true;
	}

	private DateTime GetListedDate()
	{
		return _itemService.GetItemById(itemId)?.ListedDate ?? DateTime.Now;
	}
}