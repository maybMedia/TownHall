namespace TownHall
{
	public interface IItemRepository
	{
		public Item GetById(Guid id);

		public List<Item> GetByName(string name);

		public List<Item> GetByUser(Guid userId);

		public void AddItem(Item newItem);

		public void UpdateItem(Item item);
	}
}
