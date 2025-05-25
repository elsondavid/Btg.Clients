using Btg.Clients.Models;
using Btg.Clients.Services;
using Btg.Clients.Views.EditClients;

namespace Btg.Clients.Views.ListClients;

public class ListClientPageViewModel : ViewModelBase
{
    private readonly IClientService _clientService;
    private readonly INavigationService _navigationService;
    private readonly IServiceProvider _serviceProvider;

    public ListClientPageViewModel(IClientService clientService, 
                                   INavigationService navigationService,
                                   IServiceProvider serviceProvider)
    {
        _clientService = clientService;
        _navigationService = navigationService;
        _serviceProvider = serviceProvider;
        Clients = new ReactiveCollection<ListClientItem>();
        NewClientCommand = new AsyncReactiveCommand().WithSubscribe(AddNewClient);

        clientService.ObserveInserted.Subscribe(OnClientInserted).AddTo(Disposables);
        clientService.ObserveUpdated.Subscribe(OnClientUpdated).AddTo(Disposables);
        clientService.ObserveDeleted.Subscribe(OnClientDeleted).AddTo(Disposables);
    }

    public ReactiveCollection<ListClientItem> Clients { get; }
    public AsyncReactiveCommand NewClientCommand { get; }

    public override void OnInitialize(INavigationParameters parameters)
    {
        var clients = _clientService.GetClients()
            .Select(model =>
            {
                var item = _serviceProvider.GetRequiredService<ListClientItem>().AddTo(Disposables);
                item.Initialize(model);
                return item;
            });

        Clients.AddRangeOnScheduler(clients);
    }

    private void OnClientInserted(ClientModel model)
    {
        var item = _serviceProvider.GetRequiredService<ListClientItem>().AddTo(Disposables);
        item.Initialize(model);
        Clients.Add(item);
    }

    private void OnClientUpdated(ClientModel updatedModel)
    {
        var client = Clients.Single(x => x.Id.Value == updatedModel.Id);
        client.Name.Value = updatedModel.Name;
        client.Lastname.Value = updatedModel.Lastname;
        client.Age.Value = updatedModel.Age;
        client.Address.Value = updatedModel.Address;
    }

    private void OnClientDeleted(Guid clientId)
    {
        var client = Clients.Single(x => x.Id.Value == clientId);
        Clients.Remove(client);
        client.Dispose();
    }

    private async Task AddNewClient()
    {
        await _navigationService.NavigateAsync<EditClientPageViewModel>();
    }
}