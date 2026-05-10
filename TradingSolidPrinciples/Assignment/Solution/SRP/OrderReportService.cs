// ════════════════════════════════════════════════════════════════════
//  SOLUTION — Task 1: SRP
//
//  Single Responsibility: generate reports and exports only.
//  Reason to change: only if report format or export format changes.
// ════════════════════════════════════════════════════════════════════

namespace TradingSolidPrinciples.Assignment.Solution.SRP;

public class OrderReportService
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
