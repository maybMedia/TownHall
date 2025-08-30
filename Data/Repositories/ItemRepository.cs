namespace TownHall
{
	public class ItemRepository : Repository<Item>, IItemRepository
	{

		public ItemRepository(TownHallContext context) : base(context)
		{
		}

		public TownHallContext TownHallContext
		{
			get { return _context as TownHallContext; }
		}

		public Item GetById(Guid id)
		{
			return TownHallContext.Items.FirstOrDefault(i => i.Id == id);
		}

		public List<Item> GetAll()
		{
			return TownHallContext.Items.ToList();
		}

		public void AddItem(Item newItem)
		{
			TownHallContext.Add(newItem);
			SaveChanges();
		}

		public void UpdateItem(Item item)
		{
			TownHallContext.Update(item);
			SaveChanges();
		}

		public void DeleteItem(Item item)
		{
			TownHallContext.Remove(item);
			SaveChanges();
		}
	}
}
