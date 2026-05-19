namespace SD_PD_IDA_46.LSP;

// =====================================================================
// LSP — COVARIANCE (Return Types)
// A subtype's overriding method may return a MORE DERIVED type than
// the base method declares. This is safe because the caller asked for
// a base type and gets something that is at least that — no surprise.
// C# supports covariant return types natively since C# 9.
// =====================================================================

public class TradeReport
{
    public string Summary { get; init; } = string.Empty;
}

public class DetailedTradeReport : TradeReport
{
    public string[] LineItems { get; init; } = [];
}

public abstract class ReportGenerator
{
    // Base return type: TradeReport
    public abstract TradeReport Generate();
}

// ✅ CovariantReportGenerator returns DetailedTradeReport — a MORE DERIVED type.
//    Every caller that expects TradeReport is still satisfied (it IS a TradeReport).
//    But callers that know about DetailedReportGenerator can use the richer type.
public sealed class DetailedReportGenerator : ReportGenerator
{
    // Covariant return: more derived than the base declaration
    public override DetailedTradeReport Generate() => new()
    {
        Summary   = "Detailed report",
        LineItems = ["EUR/USD x100 @ 1.12", "GBP/USD x200 @ 1.27"]
    };
}

public static class CovarianceDemo
{
    public static void Run()
    {
        Console.WriteLine("--- Covariance (Return Type) ---");

        // Caller using the base abstraction — perfectly substitutable
        ReportGenerator generator = new DetailedReportGenerator();
        TradeReport report = generator.Generate();
        Console.WriteLine($"  ✅ Got report: '{report.Summary}'");

        // Caller that knows the concrete type can access the richer return
        var detailedGenerator = new DetailedReportGenerator();
        DetailedTradeReport detailed = detailedGenerator.Generate();
        Console.WriteLine($"  ✅ Line items: {detailed.LineItems.Length}");
    }
}
