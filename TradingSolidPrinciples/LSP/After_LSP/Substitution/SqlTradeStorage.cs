// ╔══════════════════════════════════════════════════════════════════╗
//  LSP — SUBSTITUTION RULE  (After: correct writer ✔)
//
//  SqlTradeStorage only implements ITradeWriter.
//  It fully honours every part of that contract — no surprises.
//  Any caller that holds an ITradeWriter can use this safely.
// ╚══════════════════════════════════════════════════════════════════╝

using TradingSolidPrinciples.Entities;

namespace TradingSolidPrinciples.LSP.After_LSP.Substitution;

public class SqlTradeStorage : ITradeWriter
{
    // ✔ Always persists — fully honours ITradeWriter contract
    public void Persist(IEnumerable<Trade> trades)
    {
        foreach (var trade in trades)
            Console.WriteLine($"[SQL] Storing {trade.SourceCurrency}/{trade.DestinationCurrency}");
    }
}
