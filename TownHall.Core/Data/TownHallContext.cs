using System.Diagnostics;
using Microsoft.EntityFrameworkCore;

namespace TownHall.Core
{
	public class TownHallContext : DbContext
	{
		public DbSet<User> Users { get; set; }
		public DbSet<Item> Items { get; set; }
		public DbSet<Message> Messages { get; set; }

		public string DbPath { get; }

		// constructs context
		public TownHallContext()
		{
			var folder = Environment.SpecialFolder.LocalApplicationData;
			var path = Environment.GetFolderPath(folder);
			DbPath = System.IO.Path.Join(path, "townhall.db");
			Debug.WriteLine($"[DEBUG] TownHallContext created. DB Path: {DbPath}");
		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseSqlite($"Data Source={DbPath}");
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			// seed DB
		}
	}
}
