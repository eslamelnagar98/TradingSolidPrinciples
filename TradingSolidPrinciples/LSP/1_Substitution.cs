namespace SD_PD_IDA_46.LSP;

// =====================================================================
// LSP — SUBSTITUTION
// A subtype must be usable wherever its base type is expected,
// without the caller needing to know the difference.
// =====================================================================

public abstract class TradeNotifier
{
    // Base contract: notify that a trade was executed
    public abstract void Notify(string message);
}

// ✅ EmailNotifier honours the contract — callers get a notification
public sealed class EmailNotifier : TradeNotifier
{
    public override void Notify(string message) =>
        Console.WriteLine($"[Email] {message}");
}

// ✅ SmsNotifier also honours the contract — same guarantee, different channel
public sealed class SmsNotifier : TradeNotifier
{
    public override void Notify(string message) =>
        Console.WriteLine($"[SMS] {message}");
}

// ❌ SilentNotifier VIOLATES substitution — callers expect a notification
//    but silently get nothing, breaking the implicit guarantee.
public sealed class SilentNotifier : TradeNotifier
{
    public override void Notify(string message) { /* does nothing */ }
}

public static class SubstitutionDemo
{
    // The method only knows about TradeNotifier — it is substitution-safe
    static void SendTradeAlert(TradeNotifier notifier, string message) =>
        notifier.Notify(message);

    public static void Run()
    {
        Console.WriteLine("--- Substitution ---");
        SendTradeAlert(new EmailNotifier(), "Trade EUR/USD executed");   // ✅
        SendTradeAlert(new SmsNotifier(),   "Trade EUR/USD executed");   // ✅
        // SendTradeAlert(new SilentNotifier(), "Trade EUR/USD executed");// ❌ violates LSP
    }
}
