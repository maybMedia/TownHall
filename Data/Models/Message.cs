using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TownHall
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
	}
}