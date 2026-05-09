// ╔══════════════════════════════════════════════════════════════════╗
//  LSP — SUBSTITUTION RULE  (Before: a correct implementation)
//
//  SqlTradeStorage fully honours the ITradeStorage contract.
//  Persist() always stores every trade — no surprises for callers.
// ╚══════════════════════════════════════════════════════════════════╝

using TradingSolidPrinciples.Entities;

namespace TradingSolidPrinciples.LSP.Before_LSP.Substitution;

public class SqlTradeStorage : ITradeStorage
{
    public void Persist(IEnumerable<Trade> trades)
    {
        foreach (var trade in trades)
            Console.WriteLine($"[SQL] Storing {trade.SourceCurrency}/{trade.DestinationCurrency}");
    }
}
