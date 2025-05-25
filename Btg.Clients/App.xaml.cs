using Btg.Clients.Views.ListClients;

namespace Btg.Clients;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        var serviceProvider = activationState!.Context.Services;
        var page = serviceProvider.GetRequiredService<ListClientPage>();
        var viewModel = serviceProvider.GetRequiredService<ListClientPageViewModel>();
        
        page.BindingContext = viewModel;
        viewModel.OnInitialize(null!);

        return new Window(new NavigationPage(page));
    }
}