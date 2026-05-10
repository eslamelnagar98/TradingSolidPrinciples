// ╔══════════════════════════════════════════════════════════════════════╗
//  DIP — AFTER  (The Principle Applied ✔)
//
//  ─────────────────────────────────────────────────────────────────────
//  DIP Rule 1: High-level modules must NOT depend on low-level modules.
//              Both should depend on abstractions (interfaces).
//
//  DIP Rule 2: Abstractions must NOT depend on details.
//              Details should depend on abstractions.
//  ─────────────────────────────────────────────────────────────────────
//
//  What changed:
//    ✔ TradeProcessor never uses new() for any dependency.
//    ✔ It only knows about interfaces: ILogger, ITradeStorage, ITradeDataProvider.
//    ✔ Swap FileLogger → ConsoleLogger?  Zero changes here.
//    ✔ Swap SQL → MongoDB?              Zero changes here.
//    ✔ Unit-test with fakes/mocks?      Pass them in — no real file or DB needed.
//
//  Dependency arrow now points inward (correct direction):
//    FileLogger          ──implements──► ILogger          ◄── TradeProcessor
//    AdoNetTradeStorage  ──implements──► ITradeStorage    ◄── TradeProcessor
//    StreamDataProvider  ──implements──► ITradeDataProvider ◄── TradeProcessor
//
//  ⚠ IMPORTANT — DIP is NOT the same as Dependency Injection:
//    • DIP is a PRINCIPLE  — "depend on abstractions, not concretions."
//    • Dependency Injection is a TECHNIQUE — the HOW of delivering those
//      abstractions into a class (constructor / property / IoC container).
//    • You can satisfy DIP without DI (e.g. a factory or service locator).
//    • DI is the most common and cleanest way to implement DIP.
//    → See DI_Techniques/ folder for all three injection techniques.
// ╚══════════════════════════════════════════════════════════════════════╝

using TradingSolidPrinciples.Interfaces;

namespace TradingSolidPrinciples.DIP.After_DIP;

public class TradeProcessor
{
    // ✔ Depends on ABSTRACTIONS — no concrete type in sight
    private readonly ITradeDataProvider _dataProvider;
    private readonly ITradeParser       _parser;
    private readonly ITradeStorage      _storage;

    // The abstractions are received from outside — NOT created here
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
        // Pure business logic — no infrastructure knowledge whatsoever
        var lines  = _dataProvider.GetTradeData();
        var trades = _parser.Parse(lines);
        _storage.Presist(trades);
    }
}
