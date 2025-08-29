namespace TownHall;

public partial class Listings : PageWithNavBar
{
	private IItemService _itemService;

	public bool IsEditMode { get; set; } = false;
	public bool IsNewItem { get; set; } = true;	

	public Listings(IItemService itemService)
	{
		InitializeComponent();

		_itemService = itemService;
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

	private bool MapFieldsToItemObject(out Item newItem)
	{
		throw new NotImplementedException();
	}
}