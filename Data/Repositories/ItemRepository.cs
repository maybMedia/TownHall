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

        // methods
    }
}
