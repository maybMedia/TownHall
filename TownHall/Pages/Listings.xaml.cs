using System.ComponentModel;
using System.Runtime.CompilerServices;
using TownHall.Core;

namespace TownHall;

[QueryProperty(nameof(ItemId), "itemId")]
public partial class Listings : PageWithNavBar, INotifyPropertyChanged
{
	// need this to make the UI reactive
	public new event PropertyChangedEventHandler PropertyChanged;
	protected new void OnPropertyChanged([CallerMemberName] string propertyName = null)
	{
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}

	private IItemService _itemService;
	private IUserService _userService;

	public string ItemId { get; set; }
	private Guid itemId => Guid.Parse(ItemId);

	private byte[] _imageData;

	private bool _isEditMode = true;
	public bool IsEditMode
	{
		get => _isEditMode;
		set
		{
			if (_isEditMode != value)
			{
				_isEditMode = value;
				OnPropertyChanged(nameof(IsEditMode));
				OnPropertyChanged(nameof(IsFieldsReadOnly));
			}
		}
	}
	public bool IsFieldsReadOnly
	{
		get => !IsEditMode;
	}
	private bool _isNewItem = true;
	public bool IsNewItem { 
		get => _isNewItem; 
		set 
		{
			if (_isNewItem != value)
			{
				_isNewItem = value;
				OnPropertyChanged(nameof(IsNewItem));
				OnPropertyChanged(nameof(EnableDelete));
			}
		}
	}
	public bool EnableDelete
	{
		get => !IsNewItem;
	}

	public Listings(IItemService itemService, IUserService userService)
	{
		InitializeComponent();

		_itemService = itemService;
		_userService = userService;

		BindingContext = this;
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
			PopulateDisplayFields(item);

			IsEditMode = (item.SellerId == GlobalCurrentUser.User.Id);
		}
	}

	private void PopulateDisplayFields(Item item)
	{
		if (item.ImageData != null)
		{
			Image.Source = ImageSource.FromStream(() => new MemoryStream(item.ImageData));
			_imageData = item.ImageData;
		}
		PriceEntry.Text = item.Price.ToString();
		SummaryEntry.Text = item.Summary;
		NameEntry.Text = item.Name;
		DescriptionEntry.Text = item.Description;
		DateListedEntry.Text = item.ListedDate.ToString();

		var seller = _userService.GetUserById(item.SellerId);
		LocationEntry.Text = seller.Address;
		SellerEntry.Text = seller.FullName;
	}

	private async void OnSaveClicked(object sender, EventArgs e)
	{
		var isValid = TryMapInputToItem(out var item);
		if (IsNewItem)
		{
			if (isValid)
			{
				_itemService.AddItem(item);

				await DisplayAlert("Created Successfully", "Your listing has been created.", "OK");

				Navigation.PopAsync();
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

				Navigation.PopAsync();
			}
			else
			{
				await DisplayAlert("Error", "There was an error saving your changes. Please try again.", "OK");
			}
		}
	}

	private bool TryMapInputToItem(out Item item)
	{
		item = _itemService.GetItemById(itemId) ?? new Item();

		// collect values from interface
		var priceText = PriceEntry.Text?.Trim();
		var summary = SummaryEntry.Text?.Trim();
		var name = NameEntry.Text?.Trim();
		var description = DescriptionEntry.Text?.Trim();

		// validate
		if (!ItemMapper.ValidateTextFields(priceText, summary, name, description))
		{
			return false;
		}
		if (!ItemMapper.ValidatePrice(priceText, out var price))
		{
			return false;
		}

		// now can map
		ItemMapper.MapItem(item, IsNewItem, itemId, name, price, _imageData, summary, description);

		return true;
	}

	private void OnDeleteClicked(object sender, EventArgs e)
	{
		var item = _itemService.GetItemById(itemId);

		_itemService.DeleteItem(item);

		DisplayAlert("Deleted Successfully", "Your listing has been deleted.", "OK");

		Navigation.PopAsync();
	}

	private async void OnSelectImageClicked(object sender, EventArgs e)
	{
		var result = await FilePicker.PickAsync(new PickOptions
		{
			PickerTitle = "Pick an image",
			FileTypes = FilePickerFileType.Images
		});

		if (result != null)
		{
			using var stream = await result.OpenReadAsync();
			using var ms = new MemoryStream();
			await stream.CopyToAsync(ms);
			_imageData = ms.ToArray();

			Image.Source = ImageSource.FromStream(() => new MemoryStream(_imageData));
		}
	}
}