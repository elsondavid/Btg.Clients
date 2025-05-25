namespace Btg.Clients.Views;

public class ViewModelBase : BindableBase, IInitialize, IInitializeAsync, INavigatedAware, IDestructible
{
    public CompositeDisposable Disposables { get; } = [];

    public virtual void OnInitialize(INavigationParameters parameters)
    {
    }

    public virtual Task OnInitializeAsync(INavigationParameters parameters)
    {
        return Task.CompletedTask;
    }

    public virtual void OnNavigatedFrom(INavigationParameters navigationParameters)
    {
    }

    public virtual void OnNavigatedTo(INavigationParameters navigationParameters)
    {
    }

    public virtual void Destroy()
    {
        Disposables.Dispose();
    }
}