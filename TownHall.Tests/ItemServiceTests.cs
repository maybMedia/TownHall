using Moq;
using TownHall.Core;

namespace TownHall.Tests
{
	public class ItemServiceTests : TestBase
	{
		[Test]
		public void Test_SearchForItems_ReturnsCorrectItem_ByName()
		{
			var users = CreateTestUsers();
			var items = CreateTestItems(users);
			MockGetAllItems(items);

			var result = itemService.SearchForItems("wooden", false);

			Assert.True(result.Count == 1);

			var expectedItem = items.First();
			var actualItem = result.First();
			Assert.That(actualItem, Is.EqualTo(expectedItem));
		}

		[Test]
		public void Test_SearchForItems_ReturnsCorrectItem_BySummary()
		{
			var users = CreateTestUsers();
			var items = CreateTestItems(users);
			MockGetAllItems(items);

			var result = itemService.SearchForItems("gpu", false);

			Assert.True(result.Count == 1);

			var expectedItem = items.ElementAt(1);
			var actualItem = result.First();
			Assert.That(actualItem, Is.EqualTo(expectedItem));
		}

		[Test]
		public void Test_SearchForItems_ReturnsCorrectItem_ByDescription()
		{
			var users = CreateTestUsers();
			var items = CreateTestItems(users);
			MockGetAllItems(items);

			var result = itemService.SearchForItems("suspension", false);

			Assert.True(result.Count == 1);

			var expectedItem = items.Last();
			var actualItem = result.First();
			Assert.That(actualItem, Is.EqualTo(expectedItem));
		}

		[Test]
		public void Test_SearchForItems_ReturnsMultipleItems()
		{
			var users = CreateTestUsers();
			var items = CreateTestItems(users);
			MockGetAllItems(items);

			var result = itemService.SearchForItems("for", false);

			Assert.True(result.Count == 3);
			Assert.That(result, Is.EqualTo(items));
		}

		[Test]
		public void Test_SearchForItems_CorrectlyReturnsEmptyList()
		{
			var users = CreateTestUsers();
			var items = CreateTestItems(users);
			MockGetAllItems(items);

			var result = itemService.SearchForItems("sharknado", false);

			Assert.True(result.Count == 0);
		}

		[Test]
		public void Test_GetAllItems_ForBuyPage()
		{
			var users = CreateTestUsers();
			var items = CreateTestItems(users);
			MockGetAllItems(items);
			GlobalCurrentUser.User = users.First();

			var result = itemService.GetAllItems(false);

			Assert.True(result.Count == 1);
			Assert.That(result.First(), Is.EqualTo(items.Last()));
		}

		[Test]
		public void Test_GetAllItems_ForSellPage()
		{
			var users = CreateTestUsers();
			var items = CreateTestItems(users);
			MockGetAllItems(items);
			GlobalCurrentUser.User = users.First();

			var result = itemService.GetAllItems(true);

			Assert.True(result.Count == 2);
			Assert.That(result.First(), Is.EqualTo(items.First()));
			Assert.That(result.Last(), Is.EqualTo(items.ElementAt(1)));
		}
	}
}