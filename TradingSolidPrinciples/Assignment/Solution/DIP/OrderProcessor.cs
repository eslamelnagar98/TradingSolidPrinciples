// ════════════════════════════════════════════════════════════════════
//  SOLUTION — Task 5: Dependency Inversion Principle
//
//  Part A: Replace concrete dependencies with abstractions (interfaces).
//  Part B: Receive them via Constructor Injection.
//  Part C: Wire everything in a Setup() method (IoC container style).
//
//  DIP  = the RULE:      "Depend on interfaces, not concrete classes."
//  DI   = the TECHNIQUE: "Push those interfaces in from outside."
// ════════════════════════════════════════════════════════════════════

namespace TradingSolidPrinciples.Assignment.Solution.DIP;

public class Order
{
    public Guid    Id            { get; set; } = Guid.NewGuid();
    public string  CustomerEmail { get; set; }
    public string  OrderType     { get; set; }
    public decimal TotalAmount   { get; set; }
    public List<string> Items    { get; set; } = new();
}

// ════════════════════════════════════════════════════════════════════
//  Part A — Abstractions (the interfaces)
// ════════════════════════════════════════════════════════════════════

public interface IOrderStorage                  // abstracts WHERE orders are saved
{
    void Save(Order order);
}

public interface IOrderEmailSender              // abstracts HOW emails are sent
{
    void SendConfirmation(Order order);
}

public interface IOrderLogger                   // abstracts WHERE logs go
{
    void Log(string message);
}

// ════════════════════════════════════════════════════════════════════
//  Concrete implementations (the "details") — depend on abstractions
// ════════════════════════════════════════════════════════════════════

public class SqlOrderStorage : IOrderStorage
{
    public void Save(Order order) =>
        Console.WriteLine($"[SQL] Order {order.Id} saved.");
}

public class SmtpEmailSender : IOrderEmailSender
{
    public void SendConfirmation(Order order) =>
        Console.WriteLine($"[SMTP] Confirmation sent to {order.CustomerEmail}.");
}

public class ConsoleOrderLogger : IOrderLogger
{
    public void Log(string message) =>
        Console.WriteLine($"[LOG] {DateTime.Now:O} {message}");
}

// ════════════════════════════════════════════════════════════════════
//  Part B — OrderProcessor depends ONLY on abstractions
//           Receives them via Constructor Injection
// ════════════════════════════════════════════════════════════════════

public class OrderProcessor
{
    // ✔ DIP: only interfaces — no concrete types
    private readonly IOrderStorage     _storage;
    private readonly IOrderEmailSender _emailSender;
    private readonly IOrderLogger      _logger;

    // ✔ Constructor Injection — caller decides what comes in
    public OrderProcessor(
        IOrderStorage     storage,
        IOrderEmailSender emailSender,
        IOrderLogger      logger)
    {
        _storage     = storage     ?? throw new ArgumentNullException(nameof(storage));
        _emailSender = emailSender ?? throw new ArgumentNullException(nameof(emailSender));
        _logger      = logger      ?? throw new ArgumentNullException(nameof(logger));
    }

    public void Process(Order order)
    {
        _logger.Log($"Processing order {order.Id}");

        if (order.Items.Count == 0)
        {
            _logger.Log("Skipped — no items.");
            return;
        }

        _storage.Save(order);
        _emailSender.SendConfirmation(order);
        _logger.Log($"Order {order.Id} completed.");
    }
}

// ════════════════════════════════════════════════════════════════════
//  Part C — Manual IoC wiring (what a container does automatically)
// ════════════════════════════════════════════════════════════════════

public static class CompositionRoot
{
    public static OrderProcessor Build()
    {
        // Swap any line here to change the whole behaviour.
        // OrderProcessor never changes.
        IOrderLogger      logger      = new ConsoleOrderLogger();  // ← swap to FileOrderLogger
        IOrderStorage     storage     = new SqlOrderStorage();     // ← swap to MongoOrderStorage
        IOrderEmailSender emailSender = new SmtpEmailSender();     // ← swap to SendGridSender

        return new OrderProcessor(storage, emailSender, logger);
    }

    // ── How to use ────────────────────────────────────────────────
    //
    //  var processor = CompositionRoot.Build();
    //  processor.Process(order);
    //
    //  Need to switch to MongoDB + SendGrid for production?
    //    Change Build() only — OrderProcessor is untouched.
}
