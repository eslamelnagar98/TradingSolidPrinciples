using TradingSolidPrinciples;
using TradingSolidPrinciples.DependencyInjection;
using TradingSolidPrinciples.Entities;
using TradingSolidPrinciples.Interfaces;
using TradingSolidPrinciples.Interfaces.Infrastrucure.Interfaces;
using TradingSolidPrinciples.Interfaces.Infrastrucure.Services.Query;
using TradingSolidPrinciples.TradeServices;
using TradingSolidPrinciples.TradingProcessing;
using TradingSolidPrinciples.TradingProcessing.Interfaces;
#region Using IService Collection (Register,Resolve) 
//var serviceCollection = RegisterCommonServices();
//var logger = serviceCollection.Resolve<ILogger>();
//logger.LogInformation("Hello ITI", 2023);
//var tradeProccessor = serviceCollection.Resolve<ITradeProcessor>();
//tradeProccessor.ProcessTrades(); 
#endregion
#region Using Yield
//var count = 0;
//var numbers = ProduceEvenNumbers(5);
//Console.WriteLine("Caller: about to iterate.");
//foreach (int i in numbers)
//{
//    Console.WriteLine($"Caller: {i}");
//}
//Console.WriteLine(count);
//IEnumerable<int> ProduceEvenNumbers(int upto)
//{
//    Console.WriteLine("Iterator: start.");
//    for (int i = 0; i <= upto; i += 2)
//    {
//        Console.WriteLine($"Iterator: about to yield {i}");
//        yield return i;
//        count++;
//        Console.WriteLine($"Iterator: yielded {i}");
//    }
//    Console.WriteLine("Iterator: end.");
//} 
#endregion
List<int>
static IRead<User> RetrieveReadObject()
{
    Console.WriteLine("Please Enter Entity Type");
    var input = Console.ReadLine();
    Enum.TryParse(typeof(EntityType), input, out var result);
    return (EntityType)result switch
    {
        EntityType.User => new ReadOne(),
        EntityType.Entity => new ReadTwo(),
        _ => throw new ArgumentNullException(nameof(result))
    };
}
static IServiceCollection RegisterCommonServices()
{
    var stream = File.OpenRead($@"G:\Technical Data\AllData.Log");
    var serviceCollection = new ServiceCollection();
    serviceCollection.Register<ITradeDataProvider, StreamTradeDataProvider>(
        () => new StreamTradeDataProvider(stream))
    .Register<ILogger, ConsoleLogger>()
    .Register<ITradeValidator, SimpleTradeValidator>(
        () => new SimpleTradeValidator(serviceCollection.Resolve<ILogger>()))
    .Register<ITradeMapper, SimpleTradeMapper>()
    .Register<ITradeParser, SimpleTradeParser>
        (() => new SimpleTradeParser(serviceCollection.Resolve<ITradeValidator>(), serviceCollection.Resolve<ITradeMapper>()))
    .Register<ITradeStorage, AdoNetTradeStorage>(() => new AdoNetTradeStorage(serviceCollection.Resolve<ILogger>()))
    .Register<ITradeProcessor, TradeProcessor>
        (() => new TradeProcessor(
            serviceCollection.Resolve<ITradeDataProvider>(),
            serviceCollection.Resolve<ITradeStorage>(),
            serviceCollection.Resolve<ITradeParser>())
        );

    return serviceCollection;
}