namespace TownHall.Core;

public class MessageRepository : Repository<Message>, IMessageRepository
{
	public MessageRepository(TownHallContext context) : base(context)
	{
	}

	public TownHallContext TownHallContext
	{
		get { return _context as TownHallContext; }
	}

	public void Add(Message message)
	{
		_context.Messages.Add(message);
		_context.SaveChanges();
	}

	public List<Message> GetMessagesByBuyerAndItem(Guid buyerId, Guid itemId)
	{
		return TownHallContext.Messages.Where(m => m.BuyerId == buyerId && m.ItemId == itemId).ToList();
	}

	public IQueryable<Message> GetMessagesByUser(Guid userId)
	{
		return TownHallContext.Messages.Where(m => m.BuyerId == userId || m.SellerId == userId);
	}

	public List<Message> getAllMessages()
	{
		return TownHallContext.Messages.ToList();
	}
}