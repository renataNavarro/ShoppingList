using Microsoft.Extensions.Logging;
using SmartGrocery.Core.Interfaces;
using SmartGrocery.App.ViewModels;
using SmartGrocery.App.Views;
using SmartGrocery.Infrastructure.Data;
using SmartGrocery.Infrastructure.Services;

namespace SmartGrocery.App;

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

#if DEBUG
		builder.Logging.AddDebug();
#endif
// caminho do DB
        var dbPath = Path.Combine(FileSystem.AppDataDirectory, "smartgrocery.db3");
		Console.WriteLine($"DB PATH: {dbPath}");

        builder.Services.AddSingleton(new LocalDatabase(dbPath));

        // repo e services
        builder.Services.AddSingleton<IRepository, SqliteRepository>();
        builder.Services.AddSingleton<IRecommendationService, LocalRecommendationService>();

        // views and viewmodels
		builder.Services.AddSingleton<HomePage>();
		builder.Services.AddTransient<ShoppingListPage>();
        builder.Services.AddSingleton<HomeViewModel>();
		builder.Services.AddTransient<ShoppingListViewModel>();

		return builder.Build();
	}
} 
