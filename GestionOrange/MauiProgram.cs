using CommunityToolkit.Maui.Maps;
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
                .UseMauiCommunityToolkitMaps("blCELDGTRKpQkSPwQ5sm~w0h-u1NLiwBgkYwEHdIPvw~Aoik9UqhqVjDJnfkQu6sguxDValzaOyH7s5NYL-2HnxECTPwKEIrsnl6arbSh4t2")
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
            
            builder.Services.AddTransient<DataPageAddUpdatesChambreViewModels>();
            builder.Services.AddTransient<DataPageAddUpdateChambre>();
            

            return builder.Build();
        }
    }
}
