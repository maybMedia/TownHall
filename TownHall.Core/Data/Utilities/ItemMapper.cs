using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace TownHall.Core
{
	public static class ItemMapper
	{
		public static bool ValidateTextFields(string priceText, string summary, string name, string description)
		{
			if (string.IsNullOrEmpty(priceText) ||
			string.IsNullOrEmpty(summary) ||
			string.IsNullOrEmpty(name) ||
			string.IsNullOrEmpty(description))
			{
				return false;
			}

			return true;
		}

		public static bool ValidatePrice(string priceText, out decimal priceValue)
		{
			priceValue = 0;

			if (!decimal.TryParse(priceText, out decimal price))
			{
				return false;
			}

			priceValue = price;
			return true;
		}

		public static void MapItem(Item item, bool isNewItem, Guid itemId, string name, decimal price, byte[] imageData, string summary, string description)
		{
			item.Id = isNewItem ? Guid.NewGuid() : itemId;
			item.Name = name;
			item.Price = price;
			item.ImageData = imageData;
			item.Summary = summary;
			item.Description = description;
			item.ListedDate = isNewItem ? DateTime.Now : item.ListedDate;
			item.SellerId = GlobalCurrentUser.User.Id;
		}
	}
}
