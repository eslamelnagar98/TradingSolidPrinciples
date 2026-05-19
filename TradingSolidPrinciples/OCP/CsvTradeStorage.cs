using SD_PD_IDA_46.SRP;
using SD_PD_IDA_46.SRP.Models;

namespace SD_PD_IDA_46.OCP;

// NEW extension: persists trades to a CSV file instead of a database.
// TradeProcessor is never touched — we simply swap the storage.
public sealed class CsvTradeStorage(string filePath) : ITradeStorage
{
    public void Persist(IEnumerable<Trade> trades)
    {
        var lines = trades.Select(t =>
            $"{t.SourceCurrency},{t.TargetCurrency},{t.Lots},{t.Price}");

        File.WriteAllLines(filePath, lines);
        TradeLogger.LogInfo("Trades saved to CSV file", lines.Count());
    }
}
