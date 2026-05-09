// ╔═══════════════════════════════════════════════════════════════════╗
//  AFTER OCP — Solution 2: Abstract Class  (Step 1 of 2)
//
//  TradeProcessorBase is the stable, closed foundation.
//  It wires the three shared dependencies once for all subclasses
//  and declares ProcessTrades() as abstract so each subclass
//  provides its own behaviour without touching the base.
//
//  Difference from the Interface solution:
//    • Abstract class = shared wiring + shared state in one place.
//    • Interface      = pure contract, each class wires itself.
//
//  OCP satisfied:
//   ✔ CLOSED for modification — this file is never changed to add features.
//   ✔ OPEN for extension     — inherit and override ProcessTrades().
// ╚═══════════════════════════════════════════════════════════════════╝

using TradingSolidPrinciples.Interfaces;

namespace TradingSolidPrinciples.TradingProcessing.After_OCP_Abstract;

public abstract class TradeProcessorBase
{
    protected readonly ITradeDataProvider _tradeDataProvider;
    protected readonly ITradeParser       _tradeParser;
    protected readonly ITradeStorage      _tradeStorage;

    protected TradeProcessorBase(
        ITradeDataProvider tradeDataProvider,
        ITradeParser       tradeParser,
        ITradeStorage      tradeStorage)
    {
        _tradeDataProvider = tradeDataProvider ?? throw new ArgumentNullException(nameof(tradeDataProvider));
        _tradeParser       = tradeParser       ?? throw new ArgumentNullException(nameof(tradeParser));
        _tradeStorage      = tradeStorage      ?? throw new ArgumentNullException(nameof(tradeStorage));
    }

    // Subclasses MUST provide the processing behaviour — base never implements it.
    public abstract void ProcessTrades();
}
