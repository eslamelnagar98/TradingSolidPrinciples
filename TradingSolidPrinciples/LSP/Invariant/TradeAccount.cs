namespace SD_PD_IDA_46.LSP.Invariant;

// Invariant: Balance must ALWAYS be >= 0 for the entire lifetime of any TradeAccount.
// Every subtype must honour this — that is what LSP requires for invariants.
public class TradeAccount
{
    // Protected setter so subclasses can update Balance only through controlled logic
    public decimal Balance { get; protected set; }

    public TradeAccount(decimal initialBalance)
    {
        if (initialBalance < 0)
            throw new ArgumentException("Initial balance cannot be negative.");

        Balance = initialBalance;   // invariant established at construction
    }

    public virtual void Withdraw(decimal amount)
    {
        if (amount > Balance)
            throw new InvalidOperationException("Insufficient funds.");

        Balance -= amount;
        // Invariant preserved: Balance >= 0 is guaranteed after every withdrawal
    }
}
