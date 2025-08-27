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

		public List<Item> GetByName(string name)
		{
			return TownHallContext.Items
		        .Where(i => i.Name.ToLower().Contains(name.ToLower()))
		        .ToList();
		}
	}
}
