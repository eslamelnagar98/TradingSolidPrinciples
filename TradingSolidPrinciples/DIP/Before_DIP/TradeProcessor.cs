// ╔══════════════════════════════════════════════════════════════════════╗
//  DIP — BEFORE  (The Violation ✗)
//
//  ─────────────────────────────────────────────────────────────────────
//  DIP Rule 1: High-level modules must NOT depend on low-level modules.
//              Both should depend on abstractions.
//
//  DIP Rule 2: Abstractions must NOT depend on details.
//              Details should depend on abstractions.
//  ─────────────────────────────────────────────────────────────────────
//
//  What is wrong here:
//    ✗ TradeProcessor (high-level) directly creates concrete low-level
//      objects: FileLogger, AdoNetTradeStorage, StreamTradeDataProvider.
//    ✗ It uses the keyword new() — locking itself to specific details.
//    ✗ To swap FileLogger → ConsoleLogger you MUST edit THIS class.
//    ✗ To swap SQL → MongoDB storage you MUST edit THIS class.
//    ✗ You cannot unit-test this class without a real file and a real DB.
//    ✗ High-level business logic is tightly coupled to infrastructure.
//
//  The dependency arrow points the wrong way:
//    TradeProcessor ──depends on──► FileLogger        (concrete)
//    TradeProcessor ──depends on──► AdoNetTradeStorage (concrete)
//    TradeProcessor ──depends on──► StreamTradeDataProvider (concrete)
// ╚══════════════════════════════════════════════════════════════════════╝

using TradingSolidPrinciples.Entities;

namespace TradingSolidPrinciples.DIP.Before_DIP;

public class TradeProcessor
{
    public void ProcessTrades()
    {
        // ✗ High-level class builds its own low-level dependencies
        var logger  = new FileLogger();                      // ← locked to file logging
        var storage = new AdoNetTradeStorage(logger);        // ← locked to SQL Server
        var stream  = File.OpenRead(@"C:\trades\data.csv");
        var reader  = new StreamTradeDataProvider(stream);   // ← locked to file stream

        var lines  = reader.GetTradeData();
        var trades = Parse(lines, logger);
        storage.Persist(trades);
    }

    // Business logic tangled with infrastructure — hard to test, hard to change
    private IEnumerable<Trade> Parse(IEnumerable<string> lines, FileLogger logger)
    {
        foreach (var line in lines)
        {
            var fields = line.Split(',');
            if (fields.Length != 3)
            {
                logger.LogWarning($"Skipping malformed line: {line}");
                continue;
            }
            yield return new Trade
            {
                SourceCurrency      = fields[0][..3],
                DestinationCurrency = fields[0][3..],
                Price               = decimal.Parse(fields[2])
            };
        }
    }
}

// ── Concrete low-level classes (the "details") ───────────────────────

internal class FileLogger
{
    private static readonly string _path = @"C:\logs\trade.log";
    public void LogWarning(string message) =>
        File.AppendAllText(_path, $"[WARN] {DateTime.Now:O} {message}{Environment.NewLine}");
    public void LogInformation(string message) =>
        File.AppendAllText(_path, $"[INFO] {DateTime.Now:O} {message}{Environment.NewLine}");
}

internal class AdoNetTradeStorage
{
    private readonly FileLogger _logger;
    public AdoNetTradeStorage(FileLogger logger) => _logger = logger;

    public void Persist(IEnumerable<Trade> trades)
    {
        // imagine real ADO.NET calls here
        _logger.LogInformation($"Stored {trades.Count()} trade(s) to SQL Server.");
    }
}

internal class StreamTradeDataProvider
{
    private readonly Stream _stream;
    public StreamTradeDataProvider(Stream stream) => _stream = stream;

    public IEnumerable<string> GetTradeData()
    {
        using var reader = new StreamReader(_stream);
        string? line;
        while ((line = reader.ReadLine()) is not null)
            yield return line;
    }
}
