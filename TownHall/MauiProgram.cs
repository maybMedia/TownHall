using Microsoft.Extensions.Logging;
using TownHall.Core;

namespace TownHall
{
	public static class MauiProgram
	{
		public static MauiApp CreateMauiApp()
		{
			var builder = MauiApp.CreateBuilder();
			builder
				.UseMauiApp<App>()
				.ConfigureFonts(fonts =>
				{
					fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
					fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
				});

			builder.Services
				.AddDbContext<TownHallContext>()
				.AddScoped<IUnitOfWork, UnitOfWork>()
				.AddScoped<IItemRepository, ItemRepository>()
				.AddScoped<IItemService, ItemService>()
				.AddScoped<IUserRepository, UserRepository>()

				.AddSingleton<IUserService, UserService>();
			// ...

#if DEBUG
			builder.Logging.AddDebug();
#endif

			var app = builder.Build();

			// Now use the built app to create the scope
			using var scope = app.Services.CreateScope();
			var dbContext = scope.ServiceProvider.GetRequiredService<TownHallContext>();
			dbContext.Database.EnsureCreated();

			return app;
		}
	}
}