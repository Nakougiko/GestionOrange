using GestionOrange.Services;
using GestionOrange.ViewModels;
using GestionOrange.Views;
using Microsoft.Extensions.Logging;

namespace GestionOrange
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

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            builder.Services.AddSingleton<DatabaseContext>();

            builder.Services.AddSingleton<DataListViewModels>();
            builder.Services.AddSingleton<DataListPage>();

            builder.Services.AddTransient<DataAddUpdatesTechnicienViewModels>();
            builder.Services.AddTransient<DataPageAddUpdateTechnicien>();

            return builder.Build();
        }
    }
}
