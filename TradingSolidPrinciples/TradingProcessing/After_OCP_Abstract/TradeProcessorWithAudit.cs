// ╔═══════════════════════════════════════════════════════════════════╗
//  AFTER OCP — Solution 2: Abstract Class  (Step 2 of 2)
//
//  Same requirement: "Audit every trade as it is processed."
//
//  What we do RIGHT:
//    1. TradeProcessorBase is NEVER touched.
//    2. We inherit from it and override ProcessTrades().
//    3. The three dependencies are already wired by the base constructor.
//    4. Audit logic lives here exclusively — zero duplication.
//
//  OCP satisfied:
//   ✔ TradeProcessorBase is closed — unchanged.
//   ✔ New behaviour delivered by overriding in a new subclass.
//   ✔ The protected fields from the base are reused freely — no re-wiring.
//
//  When to prefer Abstract over Interface:
//    → Use Abstract when subclasses share real implementation or state
//      (here: the three protected dependency fields).
//    → Use Interface when classes are unrelated and share only a contract.
// ╚═══════════════════════════════════════════════════════════════════╝

using TradingSolidPrinciples.Entities;

namespace TradingSolidPrinciples.TradingProcessing.After_OCP_Abstract;

using TradingSolidPrinciples.Interfaces;

public class TradeProcessorWithAudit : TradeProcessorBase
{
    private readonly List<string> _auditLog = new();
    private readonly List<string> _errors   = new();

    public TradeProcessorWithAudit(
        ITradeDataProvider tradeDataProvider,
        ITradeParser       tradeParser,
        ITradeStorage      tradeStorage)
        : base(tradeDataProvider, tradeParser, tradeStorage) { }

    // Base class dependency fields are inherited — no re-wiring needed.
    public override void ProcessTrades()
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
