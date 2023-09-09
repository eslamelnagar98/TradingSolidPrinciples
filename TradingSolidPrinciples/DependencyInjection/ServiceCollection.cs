namespace TradingSolidPrinciples.DependencyInjection;
public class ServiceCollection : IServiceCollection
{
    private readonly Dictionary<Type, Type> _iocContainer = new();
    public IServiceCollection Register<TFrom, TTo>()
        where TFrom : class
        where TTo : TFrom
    {
        if (!_iocContainer.ContainsKey(typeof(TFrom)))
        {
            _iocContainer.Add(typeof(TFrom), typeof(TTo));
            return this;
        }
        _iocContainer[typeof(TFrom)] = typeof(TTo);
        return this;
    }
    public T Resolve<T>() where T : class
    {
        if (!_iocContainer.TryGetValue(typeof(T), out var objectType))
        {
            throw new ArgumentOutOfRangeException(typeof(T).Name);
        }
        return (T)Activator.CreateInstance(objectType);
    }
}
