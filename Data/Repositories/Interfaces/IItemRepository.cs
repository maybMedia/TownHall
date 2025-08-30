namespace TownHall
{
	public interface IItemRepository
	{
		public Item GetById(Guid id);

		public List<Item> GetAll();

		public void AddItem(Item newItem);

		public void UpdateItem(Item item);
	}
}
