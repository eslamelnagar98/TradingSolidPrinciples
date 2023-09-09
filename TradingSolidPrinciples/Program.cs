using TradingSolidPrinciples;
using TradingSolidPrinciples.DependencyInjection;
using TradingSolidPrinciples.Entities;
using TradingSolidPrinciples.Interfaces;
using TradingSolidPrinciples.Interfaces.Infrastrucure.Interfaces;
using TradingSolidPrinciples.Interfaces.Infrastrucure.Services.Query;
using TradingSolidPrinciples.TradeServices;
using TradingSolidPrinciples.TradingProcessing;

var serviceCollection = RegisterCommonServices();
var logger = serviceCollection.Resolve<ILogger>();
logger.LogInformation("Hello ITI", 2023);
var tradeDataProvider = serviceCollection.Resolve<ITradeDataProvider>();
var tradeParser = serviceCollection.Resolve<ITradeParser>();
var tradeStorage = serviceCollection.Resolve<ITradeStorage>();
var tradeProccessor = new TradeProcessor(tradeDataProvider, tradeStorage, tradeParser);
tradeProccessor.ProcessTrades();

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
    return new ServiceCollection()
    .Register<ITradeDataProvider, BufferTradeDataProvider>()
    .Register<ITradeParser, SimpleTradeParser>()
    .Register<ITradeStorage, AdoNetTradeStorage>()
    .Register<ITradeValidator, SimpleTradeValidator>()
    .Register<ITradeMapper, SimpleTradeMapper>()
    .Register<ILogger, FileLogger>();
}