namespace TownHall
{
	public interface IItemService
	{
		public List<Item> SearchForItems(string query);
		public List<Item> SearchForItems(string query, Guid userId);

		public List<Item> GetItemsByUser(Guid userId);
	}
}
