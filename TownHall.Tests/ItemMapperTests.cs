using TownHall.Core;

namespace TownHall.Tests;

public class ItemMapperTests : TestBase
{
	[Test]
	public void Test_ValidateTextFields_ReturnsTrue()
	{
		var price = "price";
		var summary = "summary";
		var name = "name";
		var description = "description";

		var result = ItemMapper.ValidateTextFields(price, summary, name, description);

		Assert.True(result);
	}

	[Test]
	public void Test_ValidateTextFields_ReturnsFalse()
	{
		var price = "";
		var summary = "";
		var name = "name";
		var description = "";

		var result = ItemMapper.ValidateTextFields(price, summary, name, description);

		Assert.False(result);
	}

	[Test]
	public void Test_ValidatePrice_ReturnsTrue()
	{
		var price = "50";

		var result = ItemMapper.ValidatePrice(price, out decimal priceValue);

		Assert.True(result);
	}

	[Test]
	public void Test_ValidatePrice_ReturnsFalse()
	{
		var price = "fifty";

		var result = ItemMapper.ValidatePrice(price, out decimal priceValue);

		Assert.False(result);
	}

	[Test]
	public void Test_MapItem_CorrectlyMapsFields()
	{
		var item = new Item();
		var isNewItem = true;
		var id = Guid.NewGuid();
		var name = "name";
		var price = 100m;
		var imageData = new byte[0];
		var summary = "summary";
		var description = "description";

		Assert.DoesNotThrow(() => ItemMapper.MapItem(item, isNewItem, id, name, price, imageData, summary, description));
	}
}