using Btg.Clients.Models;
using Btg.Clients.Services;
using Btg.Clients.Views.EditClients;

namespace Btg.Clients.Views.ListClients;

public class ListClientItem : ViewModelBase, IDisposable
{
    private readonly IClientService _clientService;
    private readonly INavigationService _navigationService;
    private readonly IPageDialogService _dialogService;
    private ClientModel _clientModel;

    public ListClientItem(IClientService clientService,
                          INavigationService navigationService,
                          IPageDialogService dialogService)
    {
        
        _clientService = clientService;
        _navigationService = navigationService;
        _dialogService = dialogService;

        Id = new ReactiveProperty<Guid>().AddTo(Disposables);
        Name = new ReactiveProperty<string>().AddTo(Disposables);
        Lastname = new ReactiveProperty<string>().AddTo(Disposables);
        Age = new ReactiveProperty<int?>().AddTo(Disposables);
        Address = new ReactiveProperty<string>().AddTo(Disposables);
        EditCommand = new AsyncReactiveCommand().WithSubscribe(Edit).AddTo(Disposables);
        DeleteCommand = new AsyncReactiveCommand().WithSubscribe(Delete).AddTo(Disposables);
    }

    public ReactiveProperty<Guid> Id { get; }
    public ReactiveProperty<string> Name { get; }
    public ReactiveProperty<string> Lastname { get; }
    public ReactiveProperty<int?> Age { get; }
    public ReactiveProperty<string> Address { get; }
    public AsyncReactiveCommand EditCommand { get; }
    public AsyncReactiveCommand DeleteCommand { get; }

    public void Initialize(ClientModel model)
    {
        _clientModel = model;
        Id.Value = model.Id;
        Name.Value = model.Name;
        Lastname.Value = model.Lastname;
        Age.Value = model.Age;
        Address.Value = model.Address;
    }

    private async Task Edit()
    {
        await _navigationService.NavigateAsync<EditClientPageViewModel>(_clientModel.ToNavigationParameter());
    }

    private async Task Delete()
    {
        var confirmed = await _dialogService.DisplayAlertAsync("Excluir", $"Tem certeza que deseja excluir o cliente {Name.Value}", "Sim", "Cancelar");

        if (confirmed)
        {
            _clientService.Delete(Id.Value);
        }
    }

    public void Dispose()
    {
        Destroy();
    }
}