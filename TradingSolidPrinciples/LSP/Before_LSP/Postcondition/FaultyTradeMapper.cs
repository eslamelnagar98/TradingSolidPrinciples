// ╔══════════════════════════════════════════════════════════════════╗
//  LSP — POSTCONDITION RULE  (Before: the VIOLATION ✗)
//
//  Scenario:
//    A new FaultyTradeMapper is introduced. When the trade amount
//    is below a threshold it decides to return null instead of a Trade.
//
//  Why this is an LSP violation:
//    ✗  The base guarantees a non-null Trade — always.
//    ✗  This subclass returns null for small amounts.
//    ✗  Any caller that stores the result without a null-check
//       will get a NullReferenceException at runtime.
//    ✗  The caller trusted the base's postcondition — the subclass broke it.
//
//  The subclass WEAKENED the postcondition (always Trade → sometimes null).
//  LSP says: you are only allowed to KEEP or STRENGTHEN it.
// ╚══════════════════════════════════════════════════════════════════╝

using TradingSolidPrinciples.Entities;

namespace TradingSolidPrinciples.LSP.Before_LSP.Postcondition;

public class FaultyTradeMapper : TradeMapper
{
    private const int MinimumAmount = 1_000;

    // ✗ VIOLATION — weaker postcondition: can return null
    public override Trade Map(string[] fields)
    {
        var amount = int.Parse(fields[1]);

        if (amount < MinimumAmount)
            return null;    // ← caller expected a Trade, gets null → NullReferenceException

        return base.Map(fields);
    }
}
