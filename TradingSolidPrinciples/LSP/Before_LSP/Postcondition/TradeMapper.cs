// ╔══════════════════════════════════════════════════════════════════╗
//  LSP — POSTCONDITION RULE  (Before: the base class)
//
//  Definition:
//    A subclass must NOT weaken the postcondition of a method.
//    In plain English: if the base guarantees something about the result,
//    the subclass must guarantee AT LEAST the same thing.
//    Guaranteeing LESS = LSP violation.
//
//  This base mapper guarantees:
//    • Always returns a non-null Trade.
//    • Trade.SourceCurrency and DestinationCurrency are always 3-char strings.
//    • Trade.Lots is always > 0.
//
//  See FaultyTradeMapper for what happens when a subclass breaks that guarantee.
// ╚══════════════════════════════════════════════════════════════════╝

using TradingSolidPrinciples.Entities;

namespace TradingSolidPrinciples.LSP.Before_LSP.Postcondition;

public class TradeMapper
{
    private const float LotSize = 100_000f;

    // Postcondition: NEVER returns null, Lots is always > 0
    public virtual Trade Map(string[] fields)
    {
        return new Trade
        {
            SourceCurrency      = fields[0][..3],
            DestinationCurrency = fields[0][3..],
            Lots                = int.Parse(fields[1]) / LotSize,
            Price               = decimal.Parse(fields[2])
        };
    }
}
