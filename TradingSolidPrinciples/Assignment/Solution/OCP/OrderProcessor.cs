// ════════════════════════════════════════════════════════════════════
//  SOLUTION — Task 2: OCP
//
//  OrderProcessor is CLOSED for modification.
//  It receives the discount strategy from outside — it never decides
//  which strategy to use. That decision belongs to the caller.
// ════════════════════════════════════════════════════════════════════

namespace TradingSolidPrinciples.Assignment.Solution.OCP;

public class OrderProcessor
{
    private readonly IDiscountStrategy _discountStrategy;

    // ✔ OCP: strategy is injected — no if/else chain here
    public OrderProcessor(IDiscountStrategy discountStrategy)
    {
        _discountStrategy = discountStrategy;
    }

    public void Process(Order order)
    {
        var discount    = _discountStrategy.GetDiscount(order);
        var finalAmount = order.TotalAmount - (order.TotalAmount * discount);

        Console.WriteLine(
            $"[ORDER] {order.Id} | Type: {order.OrderType} | " +
            $"Discount: {discount:P0} | Final: {finalAmount:C}");
    }
}

// ── How a caller wires it — the caller decides the strategy ──────
//
//  var processor = new OrderProcessor(new PremiumDiscountStrategy());
//  processor.Process(order);
//
//  Need VIP?  Just swap:
//  var processor = new OrderProcessor(new VipDiscountStrategy());
//  → OrderProcessor untouched.
