// ════════════════════════════════════════════════════════════════════
//  SOLUTION — Task 3: Liskov Substitution Principle
//
//  Root cause of the violation:
//    ArchiveOrderStorage extended SqlOrderStorage even though it
//    could not honour the Save() contract → threw NotSupportedException.
//
//  Fix: split into two focused interfaces.
//    IOrderWriter  → anything that WRITES orders must honour Save().
//    IOrderReader  → anything that READS orders must honour GetAll().
//
//  Now each class only implements what it can fully honour.
//  LSP is satisfied: every implementation is safely substitutable.
// ════════════════════════════════════════════════════════════════════

namespace TradingSolidPrinciples.Assignment.Solution.LSP;

public class Order
{
    public Guid    Id            { get; set; } = Guid.NewGuid();
    public string  CustomerEmail { get; set; }
    public decimal TotalAmount   { get; set; }
}

// ── Two focused contracts ─────────────────────────────────────────

public interface IOrderWriter
{
    void Save(Order order);                    // contract: always persists, never throws
}

public interface IOrderReader
{
    IEnumerable<Order> GetAll();               // contract: always returns a sequence
}

// ── SqlOrderStorage: can both write and read ──────────────────────

public class SqlOrderStorage : IOrderWriter, IOrderReader
{
    private readonly List<Order> _store = new();

    // ✔ Fully honours IOrderWriter
    public void Save(Order order)
    {
        _store.Add(order);
        Console.WriteLine($"[SQL] Order {order.Id} saved.");
    }

    // ✔ Fully honours IOrderReader
    public IEnumerable<Order> GetAll()
    {
        Console.WriteLine("[SQL] Fetching all orders...");
        return _store;
    }
}

// ── ArchiveOrderStorage: read-only — only implements IOrderReader ─

public class ArchiveOrderStorage : IOrderReader   // ✔ NOT forced to implement Save()
{
    public IEnumerable<Order> GetAll()
    {
        Console.WriteLine("[Archive] Fetching archived orders...");
        return Enumerable.Empty<Order>();
    }
}

// ── Result ────────────────────────────────────────────────────────
//  IOrderWriter caller → gets SqlOrderStorage     → always works ✔
//  IOrderReader caller → gets SqlOrderStorage     → always works ✔
//  IOrderReader caller → gets ArchiveOrderStorage → always works ✔
//  No caller ever receives a NotSupportedException.
