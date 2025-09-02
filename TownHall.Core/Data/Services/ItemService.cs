namespace TownHall.Core
{
	public class ItemService : IItemService
	{
		IUnitOfWork _unitOfWork;

		public ItemService(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public List<Item> SearchForItems(string query, bool isCurrentUsersItems)
		{
			query = query.ToLower();
			return _unitOfWork.ItemRepository.GetAll()
				.Where(i => (i.SellerId == GlobalCurrentUser.User.Id) == isCurrentUsersItems)
				.Where(i => i.Name.ToLower().Contains(query) 
						 || i.Summary.ToLower().Contains(query) 
						 || i.Description.ToLower().Contains(query))
				.ToList();
		}

		public List<Item> GetAllItems(bool isCurrentUsersItems)
		{
			return _unitOfWork.ItemRepository.GetAll()
				.Where(i => (i.SellerId == GlobalCurrentUser.User.Id) == isCurrentUsersItems)
				.ToList();
		}

		public Item GetItemById(Guid id)
		{
			return _unitOfWork.ItemRepository.GetById(id);
		}

		public void AddItem(Item newItem)
		{
			_unitOfWork.ItemRepository.AddItem(newItem);
		}

		public void UpdateItem(Item item)
		{
			_unitOfWork.ItemRepository.UpdateItem(item);
		}

		public void DeleteItem(Item item)
		{
			_unitOfWork.ItemRepository.DeleteItem(item);
		}
	}
}
