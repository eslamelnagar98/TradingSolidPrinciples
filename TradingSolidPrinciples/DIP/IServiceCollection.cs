namespace SD_PD_IDA_46.DIP;

// The contract for our manual IoC container.
// High-level code depends on THIS interface, not on ServiceCollection directly.
public interface IServiceCollection
{
    // Register: "when someone asks for TFrom, give them a TTo"
    IServiceCollection Register<TFrom, TTo>()
        where TFrom : class
        where TTo : class, TFrom;

    // Register with a factory: "when someone asks for TFrom, call this func to build it"
    IServiceCollection Register<TFrom, TTo>(Func<object> objectInitializer)
        where TFrom : class
        where TTo : class, TFrom;

    // Resolve: "give me the concrete object registered for T"
    T Resolve<T>() where T : class;
}
