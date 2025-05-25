using Btg.Clients.Models;

namespace Btg.Clients.Services;

public interface IClientService
{
    IObservable<ClientModel> ObserveInserted { get; }
    IObservable<ClientModel> ObserveUpdated { get; }
    IObservable<Guid> ObserveDeleted { get; }
    IEnumerable<ClientModel> GetClients();
    void Save(ClientModel client);
    void Delete(Guid clientId);
    void Dispose();
}

public class ClientService : IDisposable, IClientService
{
    private readonly CompositeDisposable _disposables = [];
    private readonly List<ClientModel> _clients = [];
    private readonly Subject<ClientModel> _insertedSubject;
    private readonly Subject<ClientModel> _updatedSubject;
    private readonly Subject<Guid> _deletedSubject;

    public ClientService()
    {
        _insertedSubject = new Subject<ClientModel>().AddTo(_disposables);
        _updatedSubject = new Subject<ClientModel>().AddTo(_disposables);
        _deletedSubject = new Subject<Guid>().AddTo(_disposables);
    }

    public IObservable<ClientModel> ObserveInserted => _insertedSubject;
    public IObservable<ClientModel> ObserveUpdated => _updatedSubject;
    public IObservable<Guid> ObserveDeleted => _deletedSubject;

    public IEnumerable<ClientModel> GetClients()
    {
        //Mock de registro que deveriam ser obtidos de uma Web.Api por exemplo.
        _clients.AddRange(
        [
            new ClientModel
            {
                Id = Guid.CreateVersion7(DateTimeOffset.Now),
                Name = "Eder",
                Lastname = "Santana Cardoso",
                Age = 40,
                Address = "Rua das casas numero das portas"
            },
            new ClientModel
            {
                Id = Guid.CreateVersion7(DateTimeOffset.Now),
                Name = "David",
                Lastname = "Fonseca",
                Age = 35,
                Address = "Rua das casas numero das portas"
            },
        ]);

        return _clients;
    }

    public void Save(ClientModel model)
    {
        var existingClient = _clients.SingleOrDefault(x => x.Id == model.Id);

        if (existingClient == null)
        {
            _clients.Add(model);
            _insertedSubject.OnNext(model);
        }
        else
        {
            existingClient.Age = model.Age;
            existingClient.Name = model.Name;
            existingClient.Lastname = model.Lastname;
            existingClient.Address = model.Address;
            _updatedSubject.OnNext(model);
        }
    }

    public void Delete(Guid clientId)
    {
        var client = _clients.SingleOrDefault(x => x.Id == clientId);

        if (client == null) return;

        _clients.Remove(client);
        _deletedSubject.OnNext(client.Id);
    }

    public void Dispose()
    {
        _disposables.Dispose();
    }
}