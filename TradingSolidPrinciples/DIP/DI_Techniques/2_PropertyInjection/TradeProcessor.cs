// ╔══════════════════════════════════════════════════════════════════════╗
//  DI TECHNIQUE 2 — Property Injection  (use for OPTIONAL dependencies)
//
//  How it works:
//    Dependencies are exposed as public settable properties.
//    The caller sets them after construction.
//
//  Pros:
//    ✔ Useful when a dependency is optional (has a sensible default).
//    ✔ Allows changing a dependency after construction (e.g. swap logger).
//
//  Cons:
//    ✗ Object can exist in an invalid state if a property is never set.
//    ✗ Dependencies are not explicit — harder to see what is required.
//    ✗ Risky: caller may forget to set a required property → NullRef at runtime.
//
//  Rule of thumb:
//    Use property injection ONLY for truly optional dependencies.
//    Use constructor injection for everything required.
// ╚══════════════════════════════════════════════════════════════════════╝

using TradingSolidPrinciples.Interfaces;
using TradingSolidPrinciples.TradeServices;

namespace TradingSolidPrinciples.DIP.DI_Techniques._2_PropertyInjection;

public class TradeProcessor
{
    // ── Required dependencies — still via constructor (best practice) ─
    private readonly ITradeDataProvider _dataProvider;
    private readonly ITradeParser       _parser;
    private readonly ITradeStorage      _storage;

    // ── Optional dependency — injected via property, has a safe default ─
    public ILogger Logger { get; set; } = new ConsoleLogger();   // ← default provided

    public TradeProcessor(
        ITradeDataProvider dataProvider,
        ITradeParser       parser,
        ITradeStorage      storage)
    {
        _dataProvider = dataProvider ?? throw new ArgumentNullException(nameof(dataProvider));
        _parser       = parser       ?? throw new ArgumentNullException(nameof(parser));
        _storage      = storage      ?? throw new ArgumentNullException(nameof(storage));
    }

    public void ProcessTrades()
    {
        Logger.LogInformation("Processing trades...");     // uses whatever is set
        var lines  = _dataProvider.GetTradeData();
        var trades = _parser.Parse(lines);
        _storage.Presist(trades);
        Logger.LogInformation("Done.");
    }
}

// ── How a caller uses property injection ─────────────────────────────
//
//  var processor = new TradeProcessor(dataProvider, parser, storage);
//
//  // Logger is optional — ConsoleLogger is used by default.
//  // Override only when needed:
//  processor.Logger = new FileLogger();   ← swap at any time
//
//  processor.ProcessTrades();
