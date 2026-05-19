namespace SD_PD_IDA_46.LSP;

// =====================================================================
// LSP — POSTCONDITION
// A subtype must NOT weaken (relax) the postconditions of a method.
// If the base guarantees a result > 0, the subtype must guarantee the same.
// Weakening means callers can no longer rely on the promised outcome.
// =====================================================================

public abstract class PriceCalculator
{
    // Base postcondition: returned price is always > 0
    public abstract decimal Calculate(decimal basePrice, decimal spread);
}

// ✅ SpreadCalculator strengthens the postcondition (returns > basePrice) — fine under LSP
public sealed class SpreadCalculator : PriceCalculator
{
    public override decimal Calculate(decimal basePrice, decimal spread)
    {
        var result = basePrice + spread;  // always > basePrice > 0
        return result;
    }
}

// ❌ DiscountCalculator VIOLATES LSP — it can return 0 or negative,
//    which breaks the base guarantee that the price is > 0.
public sealed class DiscountCalculator : PriceCalculator
{
    public override decimal Calculate(decimal basePrice, decimal spread)
    {
        return basePrice - spread; // can be 0 or negative — weakened postcondition!
    }
}

public static class PostconditionDemo
{
    static void PrintPrice(PriceCalculator calculator, decimal basePrice, decimal spread)
    {
        var price = calculator.Calculate(basePrice, spread);
        // caller trusts the base postcondition: price > 0
        Console.WriteLine(price > 0
            ? $"  ✅ Price = {price}"
            : $"  ❌ Violation: price {price} broke the postcondition (must be > 0)");
    }

    public static void Run()
    {
        Console.WriteLine("--- Postcondition ---");
        PrintPrice(new SpreadCalculator(),   100m, 5m);   // ✅ returns 105
        PrintPrice(new DiscountCalculator(), 100m, 150m); // ❌ returns -50 — LSP violated
    }
}
