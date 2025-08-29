namespace TownHall;

[QueryProperty(nameof(ItemId), "itemId")]
public partial class Listings : PageWithNavBar
{
	private IItemService _itemService;
	private IUserService _userService;

	public string ItemId { get; set; }
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

		if (ItemId == Guid.Empty.ToString())
		{
			IsNewItem = true;
		}
		else
		{
			IsNewItem = false;

			var item = _itemService.GetItemById(Guid.Parse(ItemId));
			PopulateEntryFields(item);

			if (item.SellerId != _userService.LoggedInUser.Id)
			{
				IsEditMode = false;
				SaveButton.IsVisible = false;
			}
			else
			{
				IsEditMode = true;
			}
		}
	}

	private void PopulateEntryFields(Item item)
	{
		PriceEntry.Placeholder = item.Price.ToString();
		SummaryEntry.Placeholder = item.Summary;
		NameEntry.Placeholder = item.Name;
		DescriptionEntry.Placeholder = item.Description;
		DateListedEntry.Placeholder = item.ListedDate.ToString();
		LocationEntry.Placeholder = item.Seller?.Address;
		SellerEntry.Placeholder = item.Seller?.FullName;
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
		item = null;

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
		item = new Item
		{
			Name = name,
			Price = price,
			Summary = summary,
			Description = description,
			ListedDate = IsNewItem ? DateTime.Now : GetListedDate(),
			IsAvailable = true, // will handle closing listings later
			Seller = _userService.LoggedInUser
		};

		return true;
	}

	private DateTime GetListedDate()
	{
		var listedDate = _itemService.GetItemById(Guid.Empty)?.ListedDate; // need to use actual id
		
		return listedDate ?? DateTime.Now;
	}
}