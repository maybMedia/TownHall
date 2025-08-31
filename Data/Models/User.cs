using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TownHall
{
	public class User
	{
		[Key]
		public Guid Id { get; set; }

		public string Password { get; set; }

		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string FullName => FirstName + " " +  LastName;

		public string Email { get; set; }
		public string Phone { get; set; }
		public string Address { get; set; }

		[NotMapped]
		public List<Item> Items { get; set; } = new List<Item>();
	}
}