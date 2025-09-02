namespace TownHall.Core;

public class MessageService : IMessageService
{
	private IUnitOfWork _unitOfWork;

	public MessageService(IUnitOfWork unitOfWork)
	{
		_unitOfWork = unitOfWork;
	}

	public void SendMessage(Message message)
	{
		_unitOfWork.MessageRepository.Add(message);
	}

	public List<Message> GetMessagesByBuyerIdAndItemId(Guid buyerId, Guid itemId)
	{
		return _unitOfWork.MessageRepository.GetMessagesByBuyerAndItem(buyerId, itemId);
	}

	public List<Conversation> getUserConversations(Guid userId)
	{
		var userMessages = _unitOfWork.MessageRepository.GetMessagesByUser(userId);

		var conversations = userMessages
			.GroupBy(m => new { m.ItemId, m.BuyerId, m.SellerId })
			.Select(g => new Conversation
			{
				ItemId = g.Key.ItemId,
				BuyerId = g.Key.BuyerId,
				SellerId = g.Key.SellerId,
				Messages = g.OrderBy(m => m.Timestamp).ToList()
			})
			.ToList();

		return conversations;
	}
}

public class Conversation
{
	public Guid ItemId { get; set; }

	public Guid BuyerId { get; set; }

	public Guid SellerId { get; set; }

	public List<Message> Messages { get; set; }
}