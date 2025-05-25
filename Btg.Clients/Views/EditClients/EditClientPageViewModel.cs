using Btg.Clients.Extensions;
using Btg.Clients.Models;
using Btg.Clients.Services;

namespace Btg.Clients.Views.EditClients;

public class EditClientPageViewModel : ViewModelBase
{
    private readonly IClientService _clientService;
    private readonly INavigationService _navigationService;
    private ClientModel _clientModel;

    public EditClientPageViewModel(IClientService clientService, INavigationService navigationService)
    {
        _clientService = clientService;
        _navigationService = navigationService;

        Name = new ReactiveProperty<string?>(mode: ReactivePropertyMode.IgnoreInitialValidationError)
            .SetValidateNotifyError(x => string.IsNullOrWhiteSpace(x) ? "O nome deve ser preenchido" : null)
            .NotifyErrorOn(x => NameErrorMessage = x)
            .AddTo(Disposables);

        Lastname = new ReactiveProperty<string?>().AddTo(Disposables);
        Age = new ReactiveProperty<int?>().AddTo(Disposables);
        Address = new ReactiveProperty<string?>().AddTo(Disposables);

        var canSave = Name.Select(x => !string.IsNullOrEmpty(x));
        SaveCommand = new ReactiveCommand(canSave, false).AddTo(Disposables);
        SaveCommand.Subscribe(_ => Save()).AddTo(Disposables);
    }

    public ReactiveProperty<string?> Name { get; }
    public ReadOnlyReactivePropertySlim<string?> NameErrorMessage { get; private set; } = null!;
    public ReactiveProperty<string?> Lastname { get; }
    public ReactiveProperty<int?> Age { get; }
    public ReactiveProperty<string?> Address { get; }
    public ReactiveCommand SaveCommand { get; }

    public override void OnInitialize(INavigationParameters parameters)
    {
        _clientModel = parameters.GetValue<ClientModel>() ?? new ClientModel{ Id = Guid.CreateVersion7(DateTimeOffset.Now) };
        Name.Value = _clientModel.Name;
        Lastname.Value = _clientModel.Lastname;
        Age.Value = _clientModel.Age;
        Address.Value = _clientModel.Address;
    }

    private void Save()
    {
        _clientModel.Name = Name.Value!;
        _clientModel.Lastname = Lastname.Value!;
        _clientModel.Age = Age.Value;
        _clientModel.Address = Address.Value!;

        _clientService.Save(_clientModel);
        _navigationService.GoBackAsync().ToObservable().Subscribe();
    }
}