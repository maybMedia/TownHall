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
}