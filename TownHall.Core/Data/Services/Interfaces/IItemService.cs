namespace TownHall.Core
{
	public interface IItemService
	{
		public List<Item> SearchForItems(string query, bool isCurrentUsersItems);

		public List<Item> GetAllItems(bool isCurrentUsersItems);

		public Item GetItemById(Guid id);

		public void AddItem(Item item);

		public void UpdateItem(Item item);

		public void DeleteItem(Item item);
	}
}
