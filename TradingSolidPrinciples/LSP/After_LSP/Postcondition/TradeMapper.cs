// ╔══════════════════════════════════════════════════════════════════╗
//  LSP — POSTCONDITION RULE  (After: same base, unchanged ✔)
//
//  Postcondition: always returns a non-null Trade with Lots > 0.
// ╚══════════════════════════════════════════════════════════════════╝

using TradingSolidPrinciples.Entities;

namespace TradingSolidPrinciples.LSP.After_LSP.Postcondition;

public class TradeMapper
{
    protected const float LotSize = 100_000f;

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
