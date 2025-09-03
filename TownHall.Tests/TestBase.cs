using Moq;
using TownHall.Core;

namespace TownHall.Tests
{
	public class TestBase
	{
		public Mock<IItemRepository> mockItemRepo;
		public Mock<IUserRepository> mockUserRepo;
		public Mock<IMessageRepository> mockMessageRepo;

		public ItemService itemService;
		public UserService userService;
		public MessageService messageService;

		[SetUp]
		public void Setup()
		{
			var mockUnitOfWork = new Mock<IUnitOfWork>();

			mockItemRepo = new Mock<IItemRepository>();
			mockUserRepo = new Mock<IUserRepository>();
			mockMessageRepo = new Mock<IMessageRepository>();

			mockUnitOfWork.Setup(x => x.ItemRepository).Returns(mockItemRepo.Object);
			mockUnitOfWork.Setup(x => x.UserRepository).Returns(mockUserRepo.Object);
			mockUnitOfWork.Setup(x => x.MessageRepository).Returns(mockMessageRepo.Object);

			itemService = new ItemService(mockUnitOfWork.Object);
			userService = new UserService(mockUnitOfWork.Object);
			messageService = new MessageService(mockUnitOfWork.Object);

			GlobalCurrentUser.User = new User() { Id = new Guid() }; // avoid null reference exceptions
		}

		public List<User> CreateTestUsers()
		{
			return new List<User>
			{
				new User
				{
					Id = Guid.NewGuid(),
					Password = "Pass@123",
					FirstName = "Alice",
					LastName = "Johnson",
					Email = "alice.johnson@example.com",
					Phone = "0401 234 567",
					Address = "12 Maple Street, Sydney NSW",
				},
				new User
				{
					Id = Guid.NewGuid(),
					Password = "Secure456!",
					FirstName = "Bob",
					LastName = "Smith",
					Email = "bob.smith@example.com",
					Phone = "0412 987 654",
					Address = "45 Ocean Avenue, Brisbane QLD",
				},
				new User
				{
					Id = Guid.NewGuid(),
					Password = "MyP@ssword789",
					FirstName = "Charlie",
					LastName = "Lee",
					Email = "charlie.lee@example.com",
					Phone = "0433 555 111",
					Address = "89 River Road, Melbourne VIC",
				}
			};
		}

		public List<Item> CreateTestItems(List<User> testUsers)
		{
			return new List<Item>() {
				new Item
				{
					Id = Guid.NewGuid(),
					Name = "Vintage Wooden Desk",
					Price = 250.00m,
					Summary = "Antique oak writing desk in good condition.",
					Description = "A classic oak desk from the 1940s, minor scratches but structurally sound. Great for a home office.",
					ListedDate = DateTime.UtcNow.AddDays(-10),
					SellerId = testUsers.First().Id,
					ImageData = null
				},
				new Item
				{
					Id = Guid.NewGuid(),
					Name = "Gaming Laptop - RTX 4070",
					Price = 1899.99m,
					Summary = "High-end gaming laptop with latest GPU.",
					Description = "Intel i9, 32GB RAM, 1TB SSD, RTX 4070 graphics card. Perfect for gaming and content creation.",
					ListedDate = DateTime.UtcNow.AddDays(-3),
					SellerId = testUsers.First().Id,
					ImageData = null
				},
				new Item
				{
					Id = Guid.NewGuid(),
					Name = "Mountain Bike",
					Price = 650.00m,
					Summary = "Lightweight mountain bike for trails.",
					Description = "27-speed, front suspension, hydraulic disc brakes, barely used. Ideal for off-road and city rides.",
					ListedDate = DateTime.UtcNow.AddDays(-1),
					SellerId = testUsers.Last().Id,
					ImageData = null
				}
			};
		}

		public List<Message> CreateTestMessages(List<User> testUsers, List<Item> testItems)
		{
			return []; // Braith seed if needed, otherwise you can delete this method
		}

		public void MockGetAllItems(List<Item> items)
		{
			mockItemRepo.Setup(x => x.GetAll()).Returns(items);
		}

		protected void MockValidateCredentials(string email, string password, User? returnUser)
		{
			mockUserRepo.Setup(repo => repo.ValidateCredentials(email, password))
				.Returns(returnUser);
		}

		protected void MockAddUser(User returnUser)
		{
			mockUserRepo.Setup(repo => repo.Add(It.IsAny<User>()))
				.Returns(returnUser);
		}

		protected void MockGetUsers(List<User> users)
		{
			mockUserRepo.Setup(repo => repo.GetUsers())
				.Returns(users);
		}

		protected void MockGetUserById(Guid id, User? returnUser)
		{
			mockUserRepo.Setup(repo => repo.GetById(id))
				.Returns(returnUser);
		}

		protected void MockAddMessage(Message message)
		{
			mockMessageRepo.Setup(repo => repo.Add(message))
				.Verifiable();
		}

		protected void MockGetMessagesByBuyerAndItem(Guid buyerId, Guid itemId, List<Message> messages)
		{
			mockMessageRepo.Setup(repo => repo.GetMessagesByBuyerAndItem(buyerId, itemId))
				.Returns(messages);
		}

		protected void MockGetMessagesByUser(Guid userId, List<Message> messages)
		{
			mockMessageRepo.Setup(repo => repo.GetMessagesByUser(userId))
				.Returns(messages.AsQueryable);
		}

	}
}