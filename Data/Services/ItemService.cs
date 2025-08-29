namespace TownHall
{
	public class ItemService : IItemService
	{
		IUnitOfWork _unitOfWork;

		public ItemService(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public List<Item> SearchForItems(string query)
		{
			return _unitOfWork.ItemRepository.GetByName(query);
		}

		public List<Item> SearchForItems(string query, Guid userId)
		{
			return _unitOfWork.ItemRepository.GetByName(query).Where(i => i.SellerId == userId).ToList();
		}

		public List<Item> GetItemsByUser(Guid userId)
		{
			return _unitOfWork.ItemRepository.GetByUser(userId);
		}
	}
}
