namespace TownHall
{
	public interface IItemRepository
	{
		public List<Item> GetByName(string name);
		public List<Item> GetByUser(Guid userId);
	}
}
