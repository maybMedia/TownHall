namespace TownHall.Core;

public interface IMessageService
{
	void SendMessage(Message message);

	List<Message> GetMessagesByBuyerIdAndItemId(Guid buyerId, Guid itemId);

	List<Conversation> getUserConversations(Guid userId);
}