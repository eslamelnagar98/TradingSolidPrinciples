namespace TradingSolidPrinciples.DependencyInjection;
public class ServiceCollection : IServiceCollection
{
    private readonly Dictionary<Type, Type> _iocContainer = new();
    private readonly Dictionary<Type, object> _objectInitalizerContainer = new();
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

    public IServiceCollection Register<TFrom, TTo>(Func<object> objectIntializer)
        where TFrom : class
        where TTo : TFrom
    {
        if (!_objectInitalizerContainer.ContainsKey(typeof(TFrom)))
        {
            _objectInitalizerContainer.Add(typeof(TFrom), objectIntializer());
            return this;
        }
        _objectInitalizerContainer[typeof(TFrom)] = objectIntializer();
        return this;

    }
    public T Resolve<T>() where T : class
    {
        if (_objectInitalizerContainer.TryGetValue(typeof(T), out var objectInitialize))
        {
            return objectInitialize as T;
        }
        if (!_iocContainer.TryGetValue(typeof(T), out var objectType))
        {
            throw new ArgumentOutOfRangeException(typeof(T).Name);
        }
        var objectInstance = Activator.CreateInstance(objectType);
        _objectInitalizerContainer.Add(typeof(T), objectInstance);
        return (T)objectInstance;
    }


}
