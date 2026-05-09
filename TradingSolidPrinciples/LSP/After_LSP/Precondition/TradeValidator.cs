// ╔══════════════════════════════════════════════════════════════════╗
//  LSP — PRECONDITION RULE  (After: same base, unchanged ✔)
//
//  The base class is identical to Before_LSP. It is never touched.
//  Precondition: amount must be > 0.
// ╚══════════════════════════════════════════════════════════════════╝

namespace TradingSolidPrinciples.LSP.After_LSP.Precondition;

public class TradeValidator
{
    // Precondition: amount must be > 0
    public virtual bool IsValidAmount(int amount)
    {
        if (amount <= 0)
            throw new ArgumentException("Amount must be greater than zero.");

        return true;
    }
}
