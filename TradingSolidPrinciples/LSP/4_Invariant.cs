namespace SD_PD_IDA_46.LSP;

// =====================================================================
// LSP — INVARIANT
// A subtype must preserve the invariants established by the base type.
// An invariant is a condition that must ALWAYS hold throughout the
// object's lifetime (e.g., balance can never go negative).
// =====================================================================

public class TradeAccount
{
    // Invariant: Balance must always be >= 0
    public decimal Balance { get; protected set; }

    public TradeAccount(decimal initialBalance)
    {
        if (initialBalance < 0)
            throw new ArgumentException("Initial balance cannot be negative.");
        Balance = initialBalance;
    }

    public virtual void Withdraw(decimal amount)
    {
        if (amount > Balance)
            throw new InvalidOperationException("Insufficient funds.");
        Balance -= amount;
        // Invariant preserved: Balance >= 0
    }
}

// ✅ PremiumTradeAccount still preserves the invariant
public sealed class PremiumTradeAccount(decimal initialBalance) : TradeAccount(initialBalance)
{
    public override void Withdraw(decimal amount)
    {
        if (amount > Balance)
            throw new InvalidOperationException("Insufficient funds — premium rules apply.");
        Balance -= amount;
        // Invariant still preserved: Balance >= 0
    }
}

// ❌ OverdraftAccount VIOLATES LSP — it breaks the invariant by allowing
//    Balance to go negative, which the base type guarantees never happens.
public sealed class OverdraftAccount(decimal initialBalance) : TradeAccount(initialBalance)
{
    public override void Withdraw(decimal amount)
    {
        Balance -= amount; // Balance can go negative — invariant broken!
    }
}

public static class InvariantDemo
{
    static void MakeWithdrawal(TradeAccount account, decimal amount)
    {
        account.Withdraw(amount);
        Console.WriteLine(account.Balance >= 0
            ? $"  ✅ Balance after withdrawal: {account.Balance}"
            : $"  ❌ Violation: Balance is {account.Balance} — invariant broken!");
    }

    public static void Run()
    {
        Console.WriteLine("--- Invariant ---");
        MakeWithdrawal(new PremiumTradeAccount(500m), 200m);  // ✅ Balance = 300
        MakeWithdrawal(new OverdraftAccount(500m),    700m);  // ❌ Balance = -200
    }
}
