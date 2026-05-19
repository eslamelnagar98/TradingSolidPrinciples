namespace SD_PD_IDA_46.LSP;

// =====================================================================
// LSP — PRECONDITION
// A subtype must NOT strengthen (tighten) the preconditions of a method.
// If the base accepts any positive lot size, the subtype must too.
// Strengthening means callers that satisfied the base contract suddenly fail.
// =====================================================================

// Base type — concrete so it can be instantiated directly for comparison
public class LotSizeValidator
{
    // Base precondition: lots must be > 0
    public virtual void ValidateLots(float lots)
    {
        if (lots <= 0)
            throw new ArgumentException("Lots must be positive.");
    }
}

// ✅ RelaxedValidator weakens (loosens) the precondition — always fine under LSP
public sealed class RelaxedValidator : LotSizeValidator
{
    public override void ValidateLots(float lots)
    {
        // accepts zero lots as well — weaker, not stronger
        if (lots < 0)
            throw new ArgumentException("Lots must be non-negative.");
    }
}

// ❌ StrictValidator VIOLATES LSP — it STRENGTHENS the precondition.
//    Callers that pass lots=0.5 (valid for the base) will now get an exception.
public sealed class StrictValidator : LotSizeValidator
{
    public override void ValidateLots(float lots)
    {
        if (lots < 10) // stronger requirement than the base!
            throw new ArgumentException("Lots must be at least 10.");
    }
}

public static class PreconditionDemo
{
    static void ProcessLots(LotSizeValidator validator, float lots)
    {
        validator.ValidateLots(lots); // caller relies on base precondition only
        Console.WriteLine($"  Lots {lots} accepted.");
    }

    public static void Run()
    {
        Console.WriteLine("--- Precondition ---");
        ProcessLots(new LotSizeValidator(), 0.5f);  // ✅ base accepts
        ProcessLots(new RelaxedValidator(), 0.5f);  // ✅ relaxed also accepts

        try { ProcessLots(new StrictValidator(), 0.5f); }  // ❌ throws — LSP violated
        catch (ArgumentException ex) { Console.WriteLine($"  ❌ Violation: {ex.Message}"); }
    }
}
