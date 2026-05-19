namespace SD_PD_IDA_46.LSP;

// =====================================================================
// LSP — CONTRAVARIANCE (Parameter / Delegate Types)
// A subtype's overriding method may accept a MORE GENERAL (base) type
// than the base method declares. This is safe because if a method can
// handle any object, it can certainly handle the specific type the
// caller passes.
// C# does NOT allow contravariant method parameters via override directly,
// but the principle is demonstrated through generic delegates and interfaces
// using the 'in' keyword (contravariant type parameter).
// =====================================================================

// --- Contravariance via generic delegates ---

// A delegate that processes a specific Trade event
public record TradeEvent(string Pair, decimal Price);
public record RiskyTradeEvent(string Pair, decimal Price, string RiskLevel) : TradeEvent(Pair, Price);

public static class ContravarianceDemo
{
    // Handler for a general TradeEvent
    static void HandleAnyTrade(TradeEvent e) =>
        Console.WriteLine($"  ✅ General handler: {e.Pair} @ {e.Price}");

    // Handler for a specific RiskyTradeEvent
    static void HandleRiskyTrade(RiskyTradeEvent e) =>
        Console.WriteLine($"  ✅ Risky handler: {e.Pair} @ {e.Price} [{e.RiskLevel}]");

    public static void Run()
    {
        Console.WriteLine("--- Contravariance (Delegate / 'in' parameter) ---");

        // Action<T> is contravariant in T (declared with 'in' internally in the CLR)
        // A handler that accepts the BASE type can be assigned to a delegate
        // expecting the DERIVED type — safe because it handles MORE, not less.
        Action<RiskyTradeEvent> riskyHandler = HandleAnyTrade; // ✅ contravariant assignment

        riskyHandler(new RiskyTradeEvent("EUR/USD", 1.12m, "High"));

        // ❌ The reverse (covariant assignment of Action) would be UNSAFE:
        // Action<TradeEvent> generalHandler = HandleRiskyTrade; // does not compile — correct!

        // --- Contravariance via 'in' interface ---
        Console.WriteLine();
        Console.WriteLine("  'in' keyword on interface type parameter:");

        ITradeHandler<RiskyTradeEvent> handler = new GeneralTradeHandler(); // ✅ contravariant
        handler.Handle(new RiskyTradeEvent("GBP/USD", 1.27m, "Medium"));
    }
}

// Contravariant generic interface — 'in' means T can only appear as input
public interface ITradeHandler<in T> where T : TradeEvent
{
    void Handle(T tradeEvent);
}

// Handles any TradeEvent (the base) — can be used wherever a
// handler of a more derived type is expected (contravariance)
public sealed class GeneralTradeHandler : ITradeHandler<TradeEvent>
{
    public void Handle(TradeEvent e) =>
        Console.WriteLine($"  ✅ GeneralTradeHandler received: {e.Pair} @ {e.Price}");
}
