// ════════════════════════════════════════════════════════════════════
//  SOLUTION — Task 1: Single Responsibility Principle
//  Shared entities used by all SRP solution classes
// ════════════════════════════════════════════════════════════════════

namespace TradingSolidPrinciples.Assignment.Solution.SRP;

public class Order
{
    public Guid     Id            { get; set; } = Guid.NewGuid();
    public string   CustomerEmail { get; set; }
    public string   OrderType     { get; set; }
    public decimal  TotalAmount   { get; set; }
    public List<OrderItem> Items  { get; set; } = new();
}

public class OrderItem
{
    public string  ProductName { get; set; }
    public int     Quantity    { get; set; }
    public decimal UnitPrice   { get; set; }
}
