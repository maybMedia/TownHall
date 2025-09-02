namespace TownHall;

[QueryProperty(nameof(ItemId), "itemId")]
public partial class Messages : PageWithNavBar
{
	public string ItemId { get; set; }
	private Guid itemId => Guid.Parse(ItemId);

	public Messages()
	{
		InitializeComponent();
	}

	private void OnConversationSelected(object? sender, SelectionChangedEventArgs e)
	{
		throw new NotImplementedException();
	}
}