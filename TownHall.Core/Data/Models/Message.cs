using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TownHall.Core
{
	public class Message
	{
		[Key]
		public Guid Id { get; set; }

		public string Content { get; set; }

		[ForeignKey("Item")]
		public Guid ItemId { get; set; }
		[NotMapped]
		public Item Item { get; set; }

		[ForeignKey("User")]
		public Guid SellerId { get; set; }
		[NotMapped]
		public User Seller { get; set; }

		[ForeignKey("User")]
		public Guid BuyerId { get; set; }
		[NotMapped]
		public User Buyer { get; set; }


		[ForeignKey("User")]
		public Guid SenderId { get; set; }
		[NotMapped]
		public User Sender { get; set; }

		public DateTime Timestamp { get; set; } = DateTime.UtcNow;
	}
}