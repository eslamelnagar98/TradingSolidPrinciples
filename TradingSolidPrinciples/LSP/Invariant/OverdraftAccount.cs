namespace SD_PD_IDA_46.LSP.Invariant;

// ❌ VIOLATION — OverdraftAccount breaks the invariant.
// It removes the guard, so Balance CAN go negative.
// A caller that holds a TradeAccount reference and assumes Balance >= 0
// will silently get corrupted state — this is the LSP invariant violation.
public sealed class OverdraftAccount(decimal initialBalance) : TradeAccount(initialBalance)
{
    public override void Withdraw(decimal amount)
    {
        // Guard removed — Balance can now go negative
        Balance -= amount;
        // Invariant BROKEN: Balance >= 0 is no longer guaranteed ❌
    }
}
