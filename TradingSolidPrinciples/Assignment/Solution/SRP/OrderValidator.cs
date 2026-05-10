// ════════════════════════════════════════════════════════════════════
//  SOLUTION — Task 1: SRP
//
//  Single Responsibility: validate order data only.
//  Reason to change: only if validation rules change.
// ════════════════════════════════════════════════════════════════════

namespace TradingSolidPrinciples.Assignment.Solution.SRP;

public class OrderValidator
{
    public bool IsValid(Order order, out string reason)
    {
        if (order.Items.Count == 0)
        {
            reason = "Order has no items.";
            return false;
        }
        if (string.IsNullOrWhiteSpace(order.CustomerEmail))
        {
            reason = "Customer email is missing.";
            return false;
        }
        reason = string.Empty;
        return true;
    }
}
