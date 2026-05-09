// ╔══════════════════════════════════════════════════════════════════╗
//  LSP — PRECONDITION RULE  (After: the FIX ✔)
//
//  The PremiumTradeValidator still adds special handling for premium
//  trades, but it does NOT strengthen the precondition.
//
//  Fix applied:
//    ✔ It accepts the exact same input as the base (amount > 0).
//    ✔ It calls base.IsValidAmount() first — honouring the base rule.
//    ✔ For amounts below the premium threshold it returns false
//      instead of throwing — same contract, richer behaviour.
//    ✔ Any caller using TradeValidator can safely swap in this class.
// ╚══════════════════════════════════════════════════════════════════╝

namespace TradingSolidPrinciples.LSP.After_LSP.Precondition;

public class PremiumTradeValidator : TradeValidator
{
    private const int PremiumThreshold = 10_000;

    // ✔ FIX — precondition unchanged: still accepts amount > 0
    public override bool IsValidAmount(int amount)
    {
        base.IsValidAmount(amount);   // honour the base rule first

        if (amount < PremiumThreshold)
        {
            Console.WriteLine(
                $"[Premium] Amount {amount} is below premium threshold " +
                $"({PremiumThreshold}). Flagged as standard trade.");
            return false;             // ← returns false, does NOT throw
        }

        return true;
    }
}
