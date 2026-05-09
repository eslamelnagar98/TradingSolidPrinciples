// =====================================================================
//  BEFORE SRP  –  Everything stuffed into ONE class
//  Problems:
//    1. Reads raw data          → should be ITradeDataProvider
//    2. Validates each line     → should be ITradeValidator
//    3. Maps fields to a Trade  → should be ITradeMapper
//    4. Logs warnings/info      → should be ILogger
//    5. Persists to a database  → should be ITradeStorage
//
//  Any single change (e.g. switch DB, change log destination,
//  change file format) forces you to touch and re-test this whole class.
// =====================================================================

using System.Data.SqlClient;

namespace TradingSolidPrinciples.Before_SRP;

public class TradeProcessor_Messy
{
    private static readonly float LotSize = 100000f;

    // Responsibility 1 + 2 + 3 + 4 + 5 all in one method
    public void ProcessTrades(string filePath)
    {
        // --- Responsibility 1: Reading data from a file ---
        var lines = File.ReadAllLines(filePath);

        var trades = new List<(string Source, string Dest, float Lots, decimal Price)>();

        var lineNumber = 1;
        foreach (var line in lines)
        {
            var fields = line.Split(',');

            // --- Responsibility 2: Validation ---
            if (fields.Length != 3)
            {
                // --- Responsibility 4: Logging (mixed into validation logic) ---
                Console.WriteLine($"[WARNING] Line {lineNumber}: malformed – {fields.Length} field(s) found, expected 3.");
                lineNumber++;
                continue;
            }

            if (fields[0].Length != 6)
            {
                Console.WriteLine($"[WARNING] Line {lineNumber}: trade currencies malformed – '{fields[0]}'.");
                lineNumber++;
                continue;
            }

            if (!int.TryParse(fields[1], out var tradeAmount))
            {
                Console.WriteLine($"[WARNING] Line {lineNumber}: trade amount not a valid integer – '{fields[1]}'.");
                lineNumber++;
                continue;
            }

            if (!decimal.TryParse(fields[2], out var tradePrice))
            {
                Console.WriteLine($"[WARNING] Line {lineNumber}: trade price not a valid decimal – '{fields[2]}'.");
                lineNumber++;
                continue;
            }

            // --- Responsibility 3: Mapping raw fields to a trade record ---
            var sourceCurrency      = fields[0].Substring(0, 3);
            var destinationCurrency = fields[0].Substring(3, 3);
            var lots                = tradeAmount / LotSize;

            trades.Add((sourceCurrency, destinationCurrency, lots, tradePrice));
            lineNumber++;
        }

        // --- Responsibility 5: Persisting to a database ---
        using var connection = new SqlConnection(
            "Data Source=.; Initial Catalog=TradingOperation; Integrated Security=True");
        connection.Open();
        using var transaction = connection.BeginTransaction();

        foreach (var trade in trades)
        {
            var command = connection.CreateCommand();
            command.Transaction  = transaction;
            command.CommandType  = System.Data.CommandType.StoredProcedure;
            command.CommandText  = "dbo.insert_trade";
            command.Parameters.AddWithValue("@sourceCurrency",      trade.Source);
            command.Parameters.AddWithValue("@destinationCurrency", trade.Dest);
            command.Parameters.AddWithValue("@lots",                trade.Lots);
            command.Parameters.AddWithValue("@price",               trade.Price);
            command.ExecuteNonQuery();
        }

        transaction.Commit();
        connection.Close();

        // --- Responsibility 4 (again): Logging after storage ---
        Console.WriteLine($"[INFO] {trades.Count} trade(s) processed successfully.");
    }
}
