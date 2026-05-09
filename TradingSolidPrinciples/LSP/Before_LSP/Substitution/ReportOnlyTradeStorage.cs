// ╔══════════════════════════════════════════════════════════════════╗
//  LSP — SUBSTITUTION RULE  (Before: the VIOLATION ✗)
//
//  Scenario:
//    A ReportOnlyTradeStorage is introduced for read-only reporting.
//    It implements ITradeStorage but Persist() does nothing useful —
//    it throws NotSupportedException because "reporting doesn't write."
//
//  Why this is an LSP violation:
//    ✗  The caller holds an ITradeStorage and calls Persist().
//    ✗  With SqlTradeStorage it works perfectly.
//    ✗  Swapping in ReportOnlyTradeStorage crashes at runtime.
//    ✗  The caller had to add an ugly type-check to protect itself:
//         if (storage is not ReportOnlyTradeStorage) storage.Persist(trades);
//    ✗  That if-check means substitution has FAILED — LSP is broken.
//
//  Root cause: ReportOnlyTradeStorage should NEVER implement ITradeStorage.
//  It only reads — it does not belong to a write contract.
//  See After_LSP/Substitution for the fix (split the interface).
// ╚══════════════════════════════════════════════════════════════════╝

using TradingSolidPrinciples.Entities;

namespace TradingSolidPrinciples.LSP.Before_LSP.Substitution;

public class ReportOnlyTradeStorage : ITradeStorage
{
    // ✗ VIOLATION — breaks the contract by throwing instead of persisting
    public void Persist(IEnumerable<Trade> trades)
        => throw new NotSupportedException(
            "ReportOnlyTradeStorage is read-only. Persist() is not supported.");

    // The only thing this class can actually do
    public IEnumerable<Trade> GetAll()
    {
        Console.WriteLine("[Report] Fetching trades for report...");
        return Enumerable.Empty<Trade>();
    }
}
