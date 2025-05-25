using Btg.Clients.Services;
using Btg.Clients.Views.EditClients;
using Btg.Clients.Views.ListClients;
using Naveasy.Core;

namespace Btg.Clients;

public static class ServiceRegistry
{
    public static MauiAppBuilder RegisterServices(this MauiAppBuilder builder)
    {
        builder.Services
            .AddSingleton<IClientService, ClientService>()
            .AddTransient<ListClientItem>()
            .AddTransientForNavigation<ListClientPage, ListClientPageViewModel>()
            .AddTransientForNavigation<EditClientPage, EditClientPageViewModel>();

        return builder;
    }
}