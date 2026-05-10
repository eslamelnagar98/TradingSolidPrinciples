// ════════════════════════════════════════════════════════════════════
//  SOLUTION — Task 1: SRP
//
//  Single Responsibility: persist orders to storage only.
//  Reason to change: only if the storage technology changes.
// ════════════════════════════════════════════════════════════════════

namespace TradingSolidPrinciples.Assignment.Solution.SRP;

public class OrderStorage
{
    public void Save(Order order) =>
        Console.WriteLine($"[DB] Order {order.Id} saved.");
}
