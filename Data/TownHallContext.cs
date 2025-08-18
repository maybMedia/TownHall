using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace TownHall
{
	public class TownHallContext : DbContext
	{
		//public DbSet<User> Users { get; set; }
		//public DbSet<Item> Items { get; set; }
		//public DbSet<Message> Messages { get; set; }

		public string DbPath { get; }

		// constructs context
		public TownHallContext()
		{
			var folder = Environment.SpecialFolder.LocalApplicationData;
			var path = Environment.GetFolderPath(folder);
			DbPath = System.IO.Path.Join(path, "townhall.db");
			Console.WriteLine("Database path:" + DbPath);
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
