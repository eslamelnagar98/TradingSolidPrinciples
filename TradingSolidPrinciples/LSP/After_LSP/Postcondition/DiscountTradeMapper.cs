// ╔══════════════════════════════════════════════════════════════════╗
//  LSP — POSTCONDITION RULE  (After: the FIX ✔)
//
//  DiscountTradeMapper applies a 10% lot discount for small trades
//  but it ALWAYS returns a valid, non-null Trade.
//
//  Fix applied:
//    ✔ Never returns null — base postcondition fully honoured.
//    ✔ Lots is always > 0 — base guarantee fully honoured.
//    ✔ Adds extra behaviour (discount) on top — postcondition STRENGTHENED.
//    ✔ Any caller using TradeMapper can safely swap in this class.
// ╚══════════════════════════════════════════════════════════════════╝

using TradingSolidPrinciples.Entities;

namespace TradingSolidPrinciples.LSP.After_LSP.Postcondition;

public class DiscountTradeMapper : TradeMapper
{
    private const int    SmallTradeThreshold = 1_000;
    private const float  DiscountRate        = 0.90f;   // 10 % off lots

    // ✔ FIX — postcondition unchanged: always returns a valid, non-null Trade
    public override Trade Map(string[] fields)
    {
        var trade  = base.Map(fields);                   // always non-null from base
        var amount = int.Parse(fields[1]);

        if (amount < SmallTradeThreshold)
        {
            trade.Lots *= DiscountRate;                  // strengthen: extra behaviour
            Console.WriteLine($"[Discount] Small trade — lots reduced by 10%.");
        }

        return trade;                                    // ← ALWAYS returns a Trade
    }
}
