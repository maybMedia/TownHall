namespace TownHall.Core;

public interface IMessageRepository
{
	void Add(Message message);

	List<Message> GetMessagesByBuyerAndItem(Guid buyerId, Guid itemId);

	IQueryable<Message> GetMessagesByUser(Guid userId);

	List<Message> getAllMessages();
}