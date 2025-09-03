using Moq;
using TownHall.Core;

namespace TownHall.Tests
{
	public class MessageServiceTests : TestBase
	{
		[Test]
		public void Test_SendMessage_CallsRepositoryAdd()
		{
			// Arrange
			var message = new Message
			{
				Id = Guid.NewGuid(),
				Content = "Test message",
				BuyerId = Guid.NewGuid(),
				SellerId = Guid.NewGuid(),
				ItemId = Guid.NewGuid(),
				Timestamp = DateTime.Now
			};

			MockAddMessage(message);

			// Act
			messageService.SendMessage(message);

			// Assert
			mockMessageRepo.Verify(repo => repo.Add(message), Times.Once);
		}

		[Test]
		public void Test_GetMessagesByBuyerIdAndItemId_ReturnsCorrectMessages()
		{
			// Arrange
			var buyerId = Guid.NewGuid();
			var itemId = Guid.NewGuid();
			var messages = CreateTestMessages(buyerId, itemId);

			MockGetMessagesByBuyerAndItem(buyerId, itemId, messages);

			// Act
			var result = messageService.GetMessagesByBuyerIdAndItemId(buyerId, itemId);

			// Assert
			Assert.That(result.Count, Is.EqualTo(messages.Count));
			Assert.That(result, Is.EqualTo(messages));
		}

		[Test]
		public void Test_GetMessagesByBuyerIdAndItemId_ReturnsEmptyList_WhenNoMessages()
		{
			// Arrange
			var buyerId = Guid.NewGuid();
			var itemId = Guid.NewGuid();
			var emptyMessages = new List<Message>();

			MockGetMessagesByBuyerAndItem(buyerId, itemId, emptyMessages);

			// Act
			var result = messageService.GetMessagesByBuyerIdAndItemId(buyerId, itemId);

			// Assert
			Assert.That(result.Count, Is.EqualTo(0));
			Assert.That(result, Is.EqualTo(emptyMessages));
		}

		[Test]
		public void Test_GetUserConversations_ReturnsCorrectConversations()
		{
			// Arrange
			var userId = Guid.NewGuid();
			var buyerId = Guid.NewGuid();
			var sellerId = Guid.NewGuid();
			var itemId1 = Guid.NewGuid();
			var itemId2 = Guid.NewGuid();

			var userMessages = new List<Message>
			{
				new Message { Id = Guid.NewGuid(), BuyerId = buyerId, SellerId = sellerId, ItemId = itemId1, Content = "Message 1", Timestamp = DateTime.Now.AddMinutes(-30) },
				new Message { Id = Guid.NewGuid(), BuyerId = buyerId, SellerId = sellerId, ItemId = itemId1, Content = "Message 2", Timestamp = DateTime.Now.AddMinutes(-20) },
				new Message { Id = Guid.NewGuid(), BuyerId = buyerId, SellerId = sellerId, ItemId = itemId2, Content = "Message 3", Timestamp = DateTime.Now.AddMinutes(-10) }
			};

			MockGetMessagesByUser(userId, userMessages);

			// Act
			var result = messageService.getUserConversations(userId);

			// Assert
			Assert.That(result.Count, Is.EqualTo(2));

			var conversation1 = result.First(c => c.ItemId == itemId1);
			Assert.That(conversation1.BuyerId, Is.EqualTo(buyerId));
			Assert.That(conversation1.SellerId, Is.EqualTo(sellerId));
			Assert.That(conversation1.Messages.Count, Is.EqualTo(2));
			Assert.That(conversation1.Messages.First().Content, Is.EqualTo("Message 1"));
			Assert.That(conversation1.Messages.Last().Content, Is.EqualTo("Message 2"));

			var conversation2 = result.First(c => c.ItemId == itemId2);
			Assert.That(conversation2.BuyerId, Is.EqualTo(buyerId));
			Assert.That(conversation2.SellerId, Is.EqualTo(sellerId));
			Assert.That(conversation2.Messages.Count, Is.EqualTo(1));
			Assert.That(conversation2.Messages.First().Content, Is.EqualTo("Message 3"));
		}

		[Test]
		public void Test_GetUserConversations_OrdersMessagesByTimestamp()
		{
			// Arrange
			var userId = Guid.NewGuid();
			var buyerId = Guid.NewGuid();
			var sellerId = Guid.NewGuid();
			var itemId = Guid.NewGuid();

			var userMessages = new List<Message>
			{
				new Message { Id = Guid.NewGuid(), BuyerId = buyerId, SellerId = sellerId, ItemId = itemId, Content = "Later message", Timestamp = DateTime.Now.AddMinutes(-10) },
				new Message { Id = Guid.NewGuid(), BuyerId = buyerId, SellerId = sellerId, ItemId = itemId, Content = "Earlier message", Timestamp = DateTime.Now.AddMinutes(-30) },
				new Message { Id = Guid.NewGuid(), BuyerId = buyerId, SellerId = sellerId, ItemId = itemId, Content = "Middle message", Timestamp = DateTime.Now.AddMinutes(-20) }
			};

			MockGetMessagesByUser(userId, userMessages);

			// Act
			var result = messageService.getUserConversations(userId);

			// Assert
			Assert.That(result.Count, Is.EqualTo(1));
			var conversation = result.First();
			Assert.That(conversation.Messages.Count, Is.EqualTo(3));
			Assert.That(conversation.Messages[0].Content, Is.EqualTo("Earlier message"));
			Assert.That(conversation.Messages[1].Content, Is.EqualTo("Middle message"));
			Assert.That(conversation.Messages[2].Content, Is.EqualTo("Later message"));
		}

		[Test]
		public void Test_GetUserConversations_ReturnsEmptyList_WhenNoMessages()
		{
			// Arrange
			var userId = Guid.NewGuid();
			var emptyMessages = new List<Message>();

			MockGetMessagesByUser(userId, emptyMessages);

			// Act
			var result = messageService.getUserConversations(userId);

			// Assert
			Assert.That(result.Count, Is.EqualTo(0));
		}

		[Test]
		public void Test_GetUserConversations_GroupsMessagesByBuyerSellerAndItem()
		{
			// Arrange
			var userId = Guid.NewGuid();
			var buyer1 = Guid.NewGuid();
			var buyer2 = Guid.NewGuid();
			var seller1 = Guid.NewGuid();
			var seller2 = Guid.NewGuid();
			var item1 = Guid.NewGuid();
			var item2 = Guid.NewGuid();

			var userMessages = new List<Message>
			{
				new Message { Id = Guid.NewGuid(), BuyerId = buyer1, SellerId = seller1, ItemId = item1, Content = "Group 1 Message 1", Timestamp = DateTime.Now.AddMinutes(-30) },
				new Message { Id = Guid.NewGuid(), BuyerId = buyer1, SellerId = seller1, ItemId = item1, Content = "Group 1 Message 2", Timestamp = DateTime.Now.AddMinutes(-25) },
				new Message { Id = Guid.NewGuid(), BuyerId = buyer2, SellerId = seller1, ItemId = item1, Content = "Group 2 Message 1", Timestamp = DateTime.Now.AddMinutes(-20) },
				new Message { Id = Guid.NewGuid(), BuyerId = buyer1, SellerId = seller2, ItemId = item2, Content = "Group 3 Message 1", Timestamp = DateTime.Now.AddMinutes(-15) }
			};

			MockGetMessagesByUser(userId, userMessages);

			// Act
			var result = messageService.getUserConversations(userId);

			// Assert
			Assert.That(result.Count, Is.EqualTo(3));

			var group1 = result.First(c => c.BuyerId == buyer1 && c.SellerId == seller1 && c.ItemId == item1);
			Assert.That(group1.Messages.Count, Is.EqualTo(2));

			var group2 = result.First(c => c.BuyerId == buyer2 && c.SellerId == seller1 && c.ItemId == item1);
			Assert.That(group2.Messages.Count, Is.EqualTo(1));

			var group3 = result.First(c => c.BuyerId == buyer1 && c.SellerId == seller2 && c.ItemId == item2);
			Assert.That(group3.Messages.Count, Is.EqualTo(1));
		}

		private List<Message> CreateTestMessages(Guid buyerId, Guid itemId)
		{
			return new List<Message>
			{
				new Message
				{
					Id = Guid.NewGuid(),
					BuyerId = buyerId,
					SellerId = Guid.NewGuid(),
					ItemId = itemId,
					Content = "Test message 1",
					Timestamp = DateTime.Now.AddMinutes(-10)
				},
				new Message
				{
					Id = Guid.NewGuid(),
					BuyerId = buyerId,
					SellerId = Guid.NewGuid(),
					ItemId = itemId,
					Content = "Test message 2",
					Timestamp = DateTime.Now.AddMinutes(-5)
				}
			};
		}
	}
}