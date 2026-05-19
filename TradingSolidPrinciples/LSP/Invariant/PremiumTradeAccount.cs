namespace SD_PD_IDA_46.LSP.Invariant;

// ✅ COMPLIANT — PremiumTradeAccount preserves the invariant.
// It overrides Withdraw but still guards against going below zero.
// Any caller holding a TradeAccount reference can use this safely.
public sealed class PremiumTradeAccount(decimal initialBalance) : TradeAccount(initialBalance)
{
    public override void Withdraw(decimal amount)
    {
        // Still enforces the same guard the base class defines
        if (amount > Balance)
            throw new InvalidOperationException("Insufficient funds — premium account rules apply.");

        Balance -= amount;
        // Invariant preserved: Balance >= 0 ✅
    }
}
