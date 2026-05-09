// ╔═══════════════════════════════════════════════════════════════════╗
//  AFTER OCP — Solution 1: Interface  (Step 1 of 2)
//
//  ITradeProcessor is the shared contract.
//  This class implements it and handles the basic processing flow.
//
//  OCP satisfied:
//   ✔ This class is CLOSED — it will never be modified for new features.
//   ✔ New behaviour (audit, cache, retry…) is delivered by a NEW class
//     that also implements ITradeProcessor.
//   ✔ Callers depend only on the interface — they never need to change.
// ╚═══════════════════════════════════════════════════════════════════╝

using TradingSolidPrinciples.Interfaces;
using TradingSolidPrinciples.TradingProcessing.Interfaces;

namespace TradingSolidPrinciples.TradingProcessing.After_OCP_Interface;

public class TradeProcessor : ITradeProcessor
{
    private readonly ITradeDataProvider _tradeDataProvider;
    private readonly ITradeParser       _tradeParser;
    private readonly ITradeStorage      _tradeStorage;

    public TradeProcessor(
        ITradeDataProvider tradeDataProvider,
        ITradeParser       tradeParser,
        ITradeStorage      tradeStorage)
    {
        _tradeDataProvider = tradeDataProvider ?? throw new ArgumentNullException(nameof(tradeDataProvider));
        _tradeParser       = tradeParser       ?? throw new ArgumentNullException(nameof(tradeParser));
        _tradeStorage      = tradeStorage      ?? throw new ArgumentNullException(nameof(tradeStorage));
    }

    public void ProcessTrades()
    {
        var lines  = _tradeDataProvider.GetTradeData();
        var trades = _tradeParser.Parse(lines);
        _tradeStorage.Presist(trades);
    }
}
