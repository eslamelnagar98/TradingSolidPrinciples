namespace SD_PD_IDA_46.DIP;

// Manual IoC container — this IS the DIP mechanism.
// It stores the mapping "interface → concrete type" and builds objects on demand.
// The rest of the app never calls `new ConcreteClass()` directly;
// it asks the container instead, staying decoupled from concretions.
public sealed class ServiceCollection : IServiceCollection
{
    // Stores interface → concrete Type mappings (for Activator-based creation)
    private readonly Dictionary<Type, Type> _typeMap = new();

    // Stores interface → already-built instance mappings (for factory-based creation)
    private readonly Dictionary<Type, object> _instanceMap = new();

    // Register by type only — the container will call Activator.CreateInstance later
    public IServiceCollection Register<TFrom, TTo>()
        where TFrom : class
        where TTo : class, TFrom
    {
        _typeMap[typeof(TFrom)] = typeof(TTo);
        return this; // fluent API — allows chaining .Register().Register()
    }

    // Register with a factory func — used when the constructor needs arguments
    // (e.g., ApiTradeDataProvider needs a url string and an ILogger)
    public IServiceCollection Register<TFrom, TTo>(Func<object> objectInitializer)
        where TFrom : class
        where TTo : class, TFrom
    {
        _instanceMap[typeof(TFrom)] = objectInitializer();
        return this;
    }

    // Resolve — give me the object registered for T
    public T Resolve<T>() where T : class
    {
        // 1. Already have a built instance? return it (singleton-like behaviour)
        if (_instanceMap.TryGetValue(typeof(T), out var existing))
            return (T)existing;

        // 2. Have a type mapping? build it, cache it, return it
        if (_typeMap.TryGetValue(typeof(T), out var concreteType))
        {
            var instance = Activator.CreateInstance(concreteType)!;
            _instanceMap[typeof(T)] = instance; // cache so we don't build twice
            return (T)instance;
        }

        throw new InvalidOperationException(
            $"Type '{typeof(T).Name}' is not registered in the container.");
    }
}
