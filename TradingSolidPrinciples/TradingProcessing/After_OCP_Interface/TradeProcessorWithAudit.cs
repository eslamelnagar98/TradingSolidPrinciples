// ╔═══════════════════════════════════════════════════════════════════╗
//  AFTER OCP — Solution 1: Interface  (Step 2 of 2)
//
//  Same requirement: "Audit every trade as it is processed."
//
//  What we do RIGHT:
//    1. TradeProcessor.cs is NEVER touched.
//    2. We create THIS new class that also implements ITradeProcessor.
//    3. Audit logic lives here exclusively — no duplication anywhere.
//
//  OCP satisfied:
//   ✔ TradeProcessor is closed — unchanged.
//   ✔ New behaviour delivered by a brand-new implementation.
//   ✔ Callers swap behaviour by injecting a different ITradeProcessor.
//     No caller code changes are needed.
// ╚═══════════════════════════════════════════════════════════════════╝

using TradingSolidPrinciples.Entities;
using TradingSolidPrinciples.Interfaces;
using TradingSolidPrinciples.TradingProcessing.Interfaces;

namespace TradingSolidPrinciples.TradingProcessing.After_OCP_Interface;

public class TradeProcessorWithAudit : ITradeProcessor
{
    private readonly ITradeDataProvider _tradeDataProvider;
    private readonly ITradeParser       _tradeParser;
    private readonly ITradeStorage      _tradeStorage;

    private readonly List<string> _auditLog = new();
    private readonly List<string> _errors   = new();

    public TradeProcessorWithAudit(
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
        try
        {
            var lines  = _tradeDataProvider.GetTradeData();
            var trades = _tradeParser.Parse(lines).ToList();

            AuditTradeData(trades);

            _tradeStorage.Presist(trades);

            _auditLog.Add($"[{DateTime.Now:O}] {trades.Count} trade(s) stored.");
        }
        catch (Exception ex)
        {
            _errors.Add($"[{DateTime.Now:O}] ERROR: {ex.Message}");
            ListErrors();
            throw;
        }
    }

    private void AuditTradeData(IEnumerable<Trade> trades)
    {
        foreach (var trade in trades)
        {
            _auditLog.Add(
                $"[{DateTime.Now:O}] AUDIT | " +
                $"{trade.SourceCurrency}/{trade.DestinationCurrency} " +
                $"Lots={trade.Lots:F2}  Price={trade.Price:F4}");
        }
    }

    private void ListErrors()
    {
        if (_errors.Count == 0)
        {
            Console.WriteLine("No errors recorded.");
            return;
        }

        Console.WriteLine($"=== {_errors.Count} error(s) ===");
        foreach (var error in _errors)
            Console.WriteLine(error);
    }
}
