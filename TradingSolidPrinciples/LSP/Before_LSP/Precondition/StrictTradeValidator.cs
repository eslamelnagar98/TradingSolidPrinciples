// ╔══════════════════════════════════════════════════════════════════╗
//  LSP — PRECONDITION RULE  (Before: the VIOLATION ✗)
//
//  Scenario:
//    The system has always accepted any amount > 0.
//    A new PremiumTradeValidator subclass is introduced for "premium" trades
//    and it now demands amount >= 10 000.
//
//  Why this is an LSP violation:
//    ✗  The base accepts amount = 500.
//    ✗  This subclass REJECTS amount = 500 → throws unexpectedly.
//    ✗  Any caller using TradeValidator can no longer safely swap
//       in StrictTradeValidator — it breaks with valid input.
//
//  The subclass STRENGTHENED the precondition (> 0 → >= 10 000).
//  LSP says: you are only allowed to KEEP or WEAKEN it.
// ╚══════════════════════════════════════════════════════════════════╝

namespace TradingSolidPrinciples.LSP.Before_LSP.Precondition;

public class StrictTradeValidator : TradeValidator
{
    private const int MinimumPremiumAmount = 10_000;

    // ✗ VIOLATION — stronger precondition than the base
    public override bool IsValidAmount(int amount)
    {
        if (amount < MinimumPremiumAmount)
            throw new ArgumentException(
                $"Premium trades require a minimum amount of {MinimumPremiumAmount}. " +
                $"Got: {amount}.");      // ← caller passed 500, which was valid for the base

        return true;
    }
}
