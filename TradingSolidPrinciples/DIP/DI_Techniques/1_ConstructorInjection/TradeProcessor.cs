// ╔══════════════════════════════════════════════════════════════════════╗
//  DI TECHNIQUE 1 — Constructor Injection  (most common ✔)
//
//  How it works:
//    All dependencies are declared as constructor parameters.
//    The caller is responsible for providing them when creating the object.
//
//  Pros:
//    ✔ Dependencies are explicit — you see them all in one place.
//    ✔ Object is always in a valid state after construction.
//    ✔ Works perfectly with IoC containers (they resolve the parameters).
//    ✔ Easiest to unit-test — just pass mocks/fakes to the constructor.
//
//  Cons:
//    ✗ Constructor grows if the class has many dependencies (design smell).
//
//  This is the RECOMMENDED technique. Use it by default.
// ╚══════════════════════════════════════════════════════════════════════╝

using TradingSolidPrinciples.Interfaces;

namespace TradingSolidPrinciples.DIP.DI_Techniques._1_ConstructorInjection;

public class TradeProcessor
{
    private readonly ITradeDataProvider _dataProvider;
    private readonly ITradeParser       _parser;
    private readonly ITradeStorage      _storage;
    private readonly ILogger            _logger;

    // ── All dependencies declared here — caller MUST supply them ─────
    public TradeProcessor(
        ITradeDataProvider dataProvider,
        ITradeParser       parser,
        ITradeStorage      storage,
        ILogger            logger)
    {
        _dataProvider = dataProvider ?? throw new ArgumentNullException(nameof(dataProvider));
        _parser       = parser       ?? throw new ArgumentNullException(nameof(parser));
        _storage      = storage      ?? throw new ArgumentNullException(nameof(storage));
        _logger       = logger       ?? throw new ArgumentNullException(nameof(logger));
    }

    public void ProcessTrades()
    {
        _logger.LogInformation("Processing trades...");
        var lines  = _dataProvider.GetTradeData();
        var trades = _parser.Parse(lines);
        _storage.Presist(trades);
        _logger.LogInformation("Done.");
    }
}

// ── How a caller uses constructor injection ───────────────────────────
//
//  var processor = new TradeProcessor(
//      new StreamTradeDataProvider(stream),   ← concrete, supplied by caller
//      new SimpleTradeParser(validator, mapper),
//      new AdoNetTradeStorage(logger),
//      new ConsoleLogger());
//
//  processor.ProcessTrades();
