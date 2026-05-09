// ╔══════════════════════════════════════════════════════════════════╗
//  LSP — SUBSTITUTION RULE  (Before: the contract)
//
//  Definition:
//    Every implementation of an interface (or subclass of a base)
//    must be safely substitutable wherever the base type is used —
//    without the caller needing to know which concrete type it has.
//
//  ITradeStorage promises: Persist() will store the trades.
//  Callers trust that promise completely.
//  See ReportOnlyTradeStorage for what happens when a type breaks it.
// ╚══════════════════════════════════════════════════════════════════╝

using TradingSolidPrinciples.Entities;

namespace TradingSolidPrinciples.LSP.Before_LSP.Substitution;

public interface ITradeStorage
{
    // Contract: stores ALL trades. Never throws NotSupportedException.
    void Persist(IEnumerable<Trade> trades);
}
