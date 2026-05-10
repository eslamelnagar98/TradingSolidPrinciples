// ╔══════════════════════════════════════════════════════════════════════╗
//  SOLID ASSIGNMENT — The Messy Class
//  Domain: E-Commerce Order Processing
//
//  This single file contains violations of ALL 5 SOLID principles.
//  Your job is to find them and fix them — see AssignmentQuestions.cs
// ╚══════════════════════════════════════════════════════════════════════╝

namespace TradingSolidPrinciples.Assignment;

// ── Entities ─────────────────────────────────────────────────────────

public class Order
{
    public Guid     Id          { get; set; } = Guid.NewGuid();
    public string   CustomerEmail { get; set; }
    public string   OrderType   { get; set; }   // "Standard", "Premium", "Bulk"
    public decimal  TotalAmount { get; set; }
    public List<OrderItem> Items { get; set; } = new();
}

public class OrderItem
{
    public string  ProductName { get; set; }
    public int     Quantity    { get; set; }
    public decimal UnitPrice   { get; set; }
}

// ── The ONE fat interface (ISP violation) ────────────────────────────

public interface IOrderService                          // ← ISP VIOLATION
{
    void   ProcessOrder(Order order);                  // ← processing concern
    void   SendConfirmationEmail(Order order);         // ← notification concern
    string GenerateReport(IEnumerable<Order> orders);  // ← reporting concern
    string ExportToCsv(IEnumerable<Order> orders);     // ← export concern
}

// ── The ONE god class (SRP + OCP + DIP violations) ───────────────────

public class OrderProcessor : IOrderService            // ← SRP VIOLATION: 5 responsibilities
{
    // ── DIP VIOLATION: constructing concrete dependencies with new() ──
    private readonly SqlOrderStorage  _storage  = new SqlOrderStorage();   // ← locked to SQL
    private readonly SmtpEmailSender  _emailer  = new SmtpEmailSender();   // ← locked to SMTP
    private readonly FileOrderLogger  _logger   = new FileOrderLogger();   // ← locked to file

    // ── Responsibility 1: Process & validate ─────────────────────────
    public void ProcessOrder(Order order)
    {
        _logger.Log($"Processing order {order.Id}");

        // Responsibility 2: Validation (should be a separate class)
        if (order.Items.Count == 0)
        {
            _logger.Log("Order has no items.");
            return;
        }
        if (string.IsNullOrWhiteSpace(order.CustomerEmail))
        {
            _logger.Log("Customer email is missing.");
            return;
        }

        // Responsibility 3: Discount calculation
        var discount    = GetDiscount(order);
        var finalAmount = order.TotalAmount - (order.TotalAmount * discount);
        _logger.Log($"Discount applied: {discount:P0}. Final: {finalAmount:C}");

        // Responsibility 4: Persist
        _storage.Save(order);

        // Responsibility 5: Send email
        SendConfirmationEmail(order);
    }

    // ── OCP VIOLATION: every new order type = edit this method ───────
    private decimal GetDiscount(Order order)
    {
        if (order.OrderType == "Standard")      // ← must edit here for every new type
            return 0.00m;
        else if (order.OrderType == "Premium")
            return 0.10m;
        else if (order.OrderType == "Bulk")
            return 0.20m;
        else
            return 0.00m;
    }

    // ── Responsibility 5: Notification (should be a separate class) ──
    public void SendConfirmationEmail(Order order)
    {
        _emailer.Send(
            to:      order.CustomerEmail,
            subject: $"Order {order.Id} Confirmed",
            body:    $"Thank you! Your order total is {order.TotalAmount:C}.");
    }

    // ── Responsibility 6: Reporting (nothing to do with processing) ──
    public string GenerateReport(IEnumerable<Order> orders)
    {
        var total   = orders.Sum(o => o.TotalAmount);
        var count   = orders.Count();
        return $"Orders: {count} | Total Revenue: {total:C}";
    }

    // ── Responsibility 7: Export (nothing to do with processing) ─────
    public string ExportToCsv(IEnumerable<Order> orders)
    {
        var lines = orders.Select(o =>
            $"{o.Id},{o.CustomerEmail},{o.OrderType},{o.TotalAmount}");
        return string.Join(Environment.NewLine, lines);
    }
}

// ── LSP VIOLATION: ArchiveStorage extends SqlOrderStorage ────────────
//    but throws NotSupportedException — breaks the base contract

public class SqlOrderStorage
{
    public virtual void Save(Order order) =>
        Console.WriteLine($"[SQL] Saved order {order.Id}");

    public virtual IEnumerable<Order> GetAll() =>
        Enumerable.Empty<Order>();
}

public class ArchiveOrderStorage : SqlOrderStorage     // ← LSP VIOLATION
{
    // Contract says Save() stores the order.
    // This class CANNOT write — it is read-only archive.
    // But it is forced to implement Save() because it inherits SqlOrderStorage.
    public override void Save(Order order)             // ← breaks caller's expectation
        => throw new NotSupportedException(
               "ArchiveOrderStorage is read-only. Save() is not supported.");

    public override IEnumerable<Order> GetAll()
    {
        Console.WriteLine("[Archive] Fetching archived orders...");
        return Enumerable.Empty<Order>();
    }
}

// ── Concrete infrastructure (details) ────────────────────────────────

public class SmtpEmailSender
{
    public void Send(string to, string subject, string body) =>
        Console.WriteLine($"[SMTP] To:{to} | Subject:{subject}");
}

public class FileOrderLogger
{
    public void Log(string message) =>
        Console.WriteLine($"[FILE LOG] {DateTime.Now:O} {message}");
}
