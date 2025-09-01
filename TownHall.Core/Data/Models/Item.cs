using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TownHall.Core
{
	public class Item
	{
		[Key]
		public Guid Id { get; set; }

		public string Name { get; set; }
		public decimal Price { get; set; }
		public string Summary { get; set; }
		public string Description { get; set; }
		public DateTime ListedDate { get; set; }
		public bool IsAvailable { get; set; }

		[ForeignKey("User")]
		public Guid SellerId { get; set; }

		[NotMapped]
		public List<Message> Messages { get; set; } = new List<Message>();

		public byte[]? ImageData { get; set; }
	}
}
