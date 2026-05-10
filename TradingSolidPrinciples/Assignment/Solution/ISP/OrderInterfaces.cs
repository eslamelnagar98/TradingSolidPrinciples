// ════════════════════════════════════════════════════════════════════
//  SOLUTION — Task 4: Interface Segregation Principle
//
//  The fat IOrderService forced every implementor to implement:
//    ProcessOrder + SendConfirmationEmail + GenerateReport + ExportToCsv
//
//  Fix: split by client need — who uses which method?
//    Processing team    → IOrderProcessor
//    Notification team  → IOrderNotifier
//    Reporting team     → IOrderReporter
//
//  Each class now implements ONLY the interface it truly supports.
//  No "throw NotImplementedException" stubs anywhere.
// ════════════════════════════════════════════════════════════════════

namespace TradingSolidPrinciples.Assignment.Solution.ISP;

public class Order
{
    public Guid    Id            { get; set; } = Guid.NewGuid();
    public string  CustomerEmail { get; set; }
    public string  OrderType     { get; set; }
    public decimal TotalAmount   { get; set; }
}

// ── Three focused interfaces ──────────────────────────────────────

public interface IOrderProcessor                        // processing concern only
{
    void ProcessOrder(Order order);
}

public interface IOrderNotifier                         // notification concern only
{
    void SendConfirmationEmail(Order order);
}

public interface IOrderReporter                         // reporting concern only
{
    string GenerateReport(IEnumerable<Order> orders);
    string ExportToCsv(IEnumerable<Order> orders);
}

// ── Implementations — each only implements what it needs ──────────

public class OrderProcessor : IOrderProcessor           // ONLY processes
{
    public void ProcessOrder(Order order) =>
        Console.WriteLine($"[PROCESS] Order {order.Id} processed.");
}

public class OrderNotifier : IOrderNotifier             // ONLY notifies
{
    public void SendConfirmationEmail(Order order) =>
        Console.WriteLine($"[EMAIL] Confirmation sent to {order.CustomerEmail}.");
}

public class OrderReporter : IOrderReporter             // ONLY reports
{
    public string GenerateReport(IEnumerable<Order> orders)
    {
        var count = orders.Count();
        var total = orders.Sum(o => o.TotalAmount);
        return $"Orders: {count} | Total Revenue: {total:C}";
    }

    public string ExportToCsv(IEnumerable<Order> orders)
    {
        var lines = orders.Select(o =>
            $"{o.Id},{o.CustomerEmail},{o.OrderType},{o.TotalAmount}");
        return string.Join(Environment.NewLine, lines);
    }
}

// ── A class CAN implement multiple focused interfaces if it needs to ─
//
//  public class FullOrderService : IOrderProcessor, IOrderNotifier
//  {
//      public void ProcessOrder(Order order) { ... }
//      public void SendConfirmationEmail(Order order) { ... }
//      // NOT forced to implement GenerateReport or ExportToCsv
//  }
