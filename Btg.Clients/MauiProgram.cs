using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using Naveasy.Extensions;
using Syncfusion.Maui.Toolkit.Hosting;

namespace Btg.Clients
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseNaveasy()
                .UseMauiCommunityToolkit()
                .ConfigureSyncfusionToolkit()
                .RegisterServices()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
