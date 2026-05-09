// ╔═══════════════════════════════════════════════════════════════════╗
//  BEFORE OCP — Step 1: The original, working TradeProcessor
//
//  This class works perfectly. A new requirement then arrives:
//  "We also need a version that audits every trade as it is processed."
//
//  The WRONG reaction (shown in TradeProcessorWithAudit.cs):
//    → Copy this entire class, rename it, and modify the copy.
//
//  The RIGHT reaction (see After_OCP_Interface / After_OCP_Abstract):
//    → Extend via an interface or abstract base. Never touch this file.
// ╚═══════════════════════════════════════════════════════════════════╝

using TradingSolidPrinciples.Interfaces;

namespace TradingSolidPrinciples.TradingProcessing.Before_OCP;

public class TradeProcessor
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
