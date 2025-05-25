namespace Btg.Clients.Extensions;

public static class ExtensionsOfReactivePropertyValidation
{
    public static ReactiveProperty<T> NotifyErrorOn<T>(this ReactiveProperty<T> source,
        Action<ReadOnlyReactivePropertySlim<string?>> propertySetter)
    {
        propertySetter(source.ObserveValidationErrorMessage().ToReadOnlyReactivePropertySlim());
        return source;
    }
}