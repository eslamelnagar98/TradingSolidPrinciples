// ╔══════════════════════════════════════════════════════════════════╗
//  LSP — SUBSTITUTION RULE  (After: correct reader ✔)
//
//  ReportTradeStorage only implements ITradeReader.
//  It is no longer forced to fake a Persist() it cannot support.
//  Any caller that holds an ITradeReader can use this safely.
//
//  Substitution now holds for both interfaces independently:
//    ITradeWriter callers → always get SqlTradeStorage (or any future writer)
//    ITradeReader callers → always get ReportTradeStorage (or any future reader)
// ╚══════════════════════════════════════════════════════════════════╝

using TradingSolidPrinciples.Entities;

namespace TradingSolidPrinciples.LSP.After_LSP.Substitution;

public class ReportTradeStorage : ITradeReader
{
    // ✔ Always returns a result — fully honours ITradeReader contract
    public IEnumerable<Trade> GetAll()
    {
        Console.WriteLine("[Report] Fetching all trades for report...");
        return Enumerable.Empty<Trade>();
    }
}
