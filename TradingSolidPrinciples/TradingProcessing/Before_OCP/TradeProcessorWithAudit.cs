// ╔═══════════════════════════════════════════════════════════════════╗
//  BEFORE OCP — Step 2: The OCP Violation  ✗
//
//  Requirement: "Audit every trade as it is processed."
//
//  What the developer did WRONG:
//    1. Opened TradeProcessor.cs and copied the entire class.
//    2. Renamed it TradeProcessorWithAudit.
//    3. Modified the copy to bolt on audit logic.
//
//  Why this is an OCP violation:
//    ✗ ProcessTrades() is duplicated — a bug fix must be applied TWICE.
//    ✗ The original class was "opened" (cloned & mutated) for a new feature.
//    ✗ Every future variation (caching, retry, metrics) creates another copy.
//    ✗ No shared contract — callers cannot swap implementations cleanly.
// ╚═══════════════════════════════════════════════════════════════════╝

using TradingSolidPrinciples.Entities;
using TradingSolidPrinciples.Interfaces;

namespace TradingSolidPrinciples.TradingProcessing.Before_OCP;

public class TradeProcessorWithAudit
{
    // ── Exact same fields as TradeProcessor ──────────────────────────
    private readonly ITradeDataProvider _tradeDataProvider;
    private readonly ITradeParser       _tradeParser;
    private readonly ITradeStorage      _tradeStorage;

    // ── Audit state bolted on top ────────────────────────────────────
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

    // ── DUPLICATED core logic from TradeProcessor (violation!) ───────
    public void ProcessTrades()
    {
        try
        {
            var lines  = _tradeDataProvider.GetTradeData();
            var trades = _tradeParser.Parse(lines).ToList();

            AuditTradeData(trades);       // ← new behaviour jammed into the copy

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

    // ── New methods added by mutating the copied class ────────────────
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
