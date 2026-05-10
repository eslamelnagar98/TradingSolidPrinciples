// ════════════════════════════════════════════════════════════════════
//  SOLUTION — Task 2: Open/Closed Principle
//
//  IDiscountStrategy is the CLOSED contract.
//  Adding a new order type = adding a new class that implements this.
//  The OrderProcessor never changes.
// ════════════════════════════════════════════════════════════════════

namespace TradingSolidPrinciples.Assignment.Solution.OCP;

public class Order
{
    public Guid    Id            { get; set; } = Guid.NewGuid();
    public string  CustomerEmail { get; set; }
    public string  OrderType     { get; set; }
    public decimal TotalAmount   { get; set; }
}

// ── The closed contract ───────────────────────────────────────────
public interface IDiscountStrategy
{
    decimal GetDiscount(Order order);   // returns a value between 0 and 1 (e.g. 0.10 = 10%)
}

// ── One class per order type — OPEN for extension ─────────────────

public class StandardDiscountStrategy : IDiscountStrategy
{
    public decimal GetDiscount(Order order) => 0.00m;   // 0% discount
}

public class PremiumDiscountStrategy : IDiscountStrategy
{
    public decimal GetDiscount(Order order) => 0.10m;   // 10% discount
}

public class BulkDiscountStrategy : IDiscountStrategy
{
    public decimal GetDiscount(Order order) => 0.20m;   // 20% discount
}

// Adding "VIP" in the future?
// → Create VipDiscountStrategy : IDiscountStrategy { return 0.30m; }
// → Zero changes to OrderProcessor.
