using Microsoft.Extensions.Logging;

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
				.AddScoped<IUserService, UserService>();
			// ...

#if DEBUG
			builder.Logging.AddDebug();
#endif

			return builder.Build();
		}
	}
}
