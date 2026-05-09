// ╔══════════════════════════════════════════════════════════════════╗
//  LSP — PRECONDITION RULE  (Before: the base class)
//
//  Definition:
//    A subclass must NOT strengthen the precondition of a method.
//    In plain English: if the base accepts certain input,
//    the subclass must accept AT LEAST the same input.
//    Demanding MORE = LSP violation.
//
//  This base validator accepts any trade amount > 0.
//  Callers are written against this rule.
//  See StrictTradeValidator for what happens when a subclass tightens it.
// ╚══════════════════════════════════════════════════════════════════╝

namespace TradingSolidPrinciples.LSP.Before_LSP.Precondition;

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
