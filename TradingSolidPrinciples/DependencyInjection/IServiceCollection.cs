namespace TradingSolidPrinciples.DependencyInjection;
public interface IServiceCollection
{
    IServiceCollection Register<TFrom, TTo>()
         where TFrom : class
        where TTo : TFrom;

    public IServiceCollection Register<TFrom, TTo>(Func<object> objectIntializer)
        where TFrom : class
        where TTo : TFrom;

    T Resolve<T>() where T : class;
}
