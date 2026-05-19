namespace SD_PD_IDA_46.LSP;

// =====================================================================
// LSP — INVARIANCE (Generic Type Parameters)
// A type parameter is INVARIANT when it is used for BOTH input and output.
// This means the type must match EXACTLY — neither a more derived nor a
// more general type is accepted.
//
// Why? If a generic type is used for reading (output), covariance would
// be safe. If it is used for writing (input), contravariance would be safe.
// When used for BOTH, neither direction is safe, so the compiler locks it
// to the exact type — that is invariance.
// =====================================================================

// --- Concrete domain types ---
public class Currency { public string Code { get; init; } = string.Empty; }
public class FiatCurrency   : Currency { }   // e.g. USD, EUR
public class CryptoCurrency : Currency { }   // e.g. BTC, ETH

// --- Invariant generic interface ---
// T appears as BOTH input (Add) and output (Get) → must be invariant (no in/out)
public interface ITradePortfolio<T> where T : Currency
{
    void Add(T currency);   // T as input  → needs contravariance
    T    Get(int index);    // T as output → needs covariance
    // Since T is needed for both, neither 'in' nor 'out' can be applied → INVARIANT
}

public sealed class FiatPortfolio : ITradePortfolio<FiatCurrency>
{
    private readonly List<FiatCurrency> _currencies = [];
    public void Add(FiatCurrency currency) => _currencies.Add(currency);
    public FiatCurrency Get(int index)     => _currencies[index];
}

public static class InvarianceDemo
{
    public static void Run()
    {
        Console.WriteLine("--- Invariance (Generic Type Parameter) ---");

        // ✅ Exact type match — always fine
        ITradePortfolio<FiatCurrency> fiatPortfolio = new FiatPortfolio();
        fiatPortfolio.Add(new FiatCurrency { Code = "USD" });
        Console.WriteLine($"  ✅ Added fiat currency: {fiatPortfolio.Get(0).Code}");

        // ❌ INVARIANCE: even though FiatCurrency derives from Currency,
        //    ITradePortfolio<FiatCurrency> cannot be assigned to ITradePortfolio<Currency>.
        //
        //    If this were allowed, someone could call portfolio.Add(new CryptoCurrency())
        //    through the ITradePortfolio<Currency> reference — corrupting the fiat-only list.
        //
        //    Uncomment the line below to see the compiler error (CS0266):
        // ITradePortfolio<Currency> portfolio = new FiatPortfolio(); // ❌ does not compile

        Console.WriteLine("  ❌ ITradePortfolio<FiatCurrency> cannot be used as");
        Console.WriteLine("     ITradePortfolio<Currency> — compiler enforces exact match.");
        Console.WriteLine();

        // --- Contrast: List<T> is also invariant for the same reason ---
        var fiats = new List<FiatCurrency> { new() { Code = "EUR" } };

        // ❌ List<FiatCurrency> is NOT assignable to List<Currency> — invariant
        // List<Currency> currencies = fiats; // does not compile

        // ✅ IEnumerable<T> is covariant ('out T') — read-only, so safe
        IEnumerable<Currency> readOnly = fiats; // ✅ compiles — only reading, no Add()
        Console.WriteLine($"  ✅ IEnumerable<Currency> = List<FiatCurrency> works (covariant 'out T')");
        Console.WriteLine($"     First item: {readOnly.First().Code}");
        Console.WriteLine();
        Console.WriteLine("  Summary:");
        Console.WriteLine("    List<T>            → invariant  (read + write)");
        Console.WriteLine("    IEnumerable<out T> → covariant  (read only)");
        Console.WriteLine("    Action<in T>       → contravariant (write only)");
    }
}
