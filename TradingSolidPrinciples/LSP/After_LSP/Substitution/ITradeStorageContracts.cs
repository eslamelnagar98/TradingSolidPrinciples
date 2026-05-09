// ╔══════════════════════════════════════════════════════════════════╗
//  LSP — SUBSTITUTION RULE  (After: FIX — split the interface ✔)
//
//  Root cause of the violation:
//    ReportOnlyTradeStorage was forced to implement ITradeStorage
//    even though it can never write. One fat interface caused the problem.
//
//  Fix: split into two focused interfaces.
//    ITradeWriter  → for anything that writes/persists trades.
//    ITradeReader  → for anything that reads/queries trades.
//
//  Now:
//    ✔ SqlTradeStorage    implements ITradeWriter  (writes only)
//    ✔ ReportTradeStorage implements ITradeReader  (reads only)
//    ✔ Neither class is forced to implement a method it cannot support.
//    ✔ Substitution holds for EACH interface independently.
// ╚══════════════════════════════════════════════════════════════════╝

using TradingSolidPrinciples.Entities;

namespace TradingSolidPrinciples.LSP.After_LSP.Substitution;

// ✔ Focused write contract
public interface ITradeWriter
{
    void Persist(IEnumerable<Trade> trades);
}

// ✔ Focused read contract
public interface ITradeReader
{
    IEnumerable<Trade> GetAll();
}
