namespace SD_PD_IDA_46.LSP.Invariant;

public static class InvariantDemo
{
    // The caller only knows about TradeAccount.
    // It assumes the invariant (Balance >= 0) is always true — as the base class promises.
    private static void MakeWithdrawal(TradeAccount account, decimal amount)
    {
        Console.WriteLine($"  Before withdrawal → Balance: {account.Balance}");

        try
        {
            account.Withdraw(amount);
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"  ⚠️  Withdrawal blocked: {ex.Message}");
            return;
        }

        // The caller trusts the invariant and acts on it
        if (account.Balance >= 0)
            Console.WriteLine($"  ✅ After withdrawal  → Balance: {account.Balance}  (invariant holds)");
        else
            Console.WriteLine($"  ❌ After withdrawal  → Balance: {account.Balance}  (invariant BROKEN — LSP violated)");
    }

    public static void Run()
    {
        Console.WriteLine("--- Invariant ---");

        // ✅ PremiumTradeAccount honours the invariant — guard is in place
        Console.WriteLine();
        Console.WriteLine("  [PremiumTradeAccount] Withdraw 200 from balance of 500:");
        MakeWithdrawal(new PremiumTradeAccount(500m), 200m);

        // ✅ Even when the amount exceeds the balance, the guard fires
        Console.WriteLine();
        Console.WriteLine("  [PremiumTradeAccount] Withdraw 700 from balance of 500:");
        MakeWithdrawal(new PremiumTradeAccount(500m), 700m);

        // ❌ OverdraftAccount silently breaks the invariant — no guard, Balance goes negative
        Console.WriteLine();
        Console.WriteLine("  [OverdraftAccount] Withdraw 700 from balance of 500:");
        MakeWithdrawal(new OverdraftAccount(500m), 700m);
    }
}
