using System.Data.SqlClient;
using TradingSolidPrinciples.Entities;
namespace TradingSolidPrinciples;
public class TradeProcessor
{
    private static readonly float _lotSize = 100000f;
    public void ProcessTrades(Stream stream)
    {
        List<string> lines = ReadTradeData(stream);
        List<Trade> trades = ParseTrades(lines);
        StoreTrades(trades);
    }

    private static List<Trade> ParseTrades(List<string> lines)
    {
        var trades = new List<Trade>();
        var lineCount = 1;
        foreach (var line in lines)
        {
            var fields = line.Split(new char[] { ',' });
            if (fields.Length != 3)
            {
                Console.WriteLine("WARN: Line {0} malformed. Only {1} field(s) found.", lineCount, fields.Length);
                continue;
            }
            if (fields[0].Length != 6)
            {
                Console.WriteLine("WARN: Trade currencies on line {0} malformed: '{1}'", lineCount, fields[0]);
                continue;
            }
            int tradeAmount;
            if (!int.TryParse(fields[1], out tradeAmount))
            {
                Console.WriteLine("WARN: Trade amount on line {0} not a valid integer:'{1}'", lineCount, fields[1]);
            }
            decimal tradePrice;
            if (!decimal.TryParse(fields[2], out tradePrice))
            {
                Console.WriteLine("WARN: Trade price on line {0} not a valid decimal: '{1}'", lineCount, fields[2]);
            }
            var sourceCurrencyCode = fields[0].Substring(0, 3);
            var destinationCurrencyCode = fields[0].Substring(3, 3);
            var trade = new Trade
            {
                SourceCurrency = sourceCurrencyCode,
                DestinationCurrency = destinationCurrencyCode,
                Lots = tradeAmount / _lotSize,
                Price = tradePrice
            };
            trades.Add(trade);
            lineCount++;
        }

        return trades;
    }

    private static List<string> ReadTradeData(Stream stream)
    {
        var lines = new List<string>();
        using (var reader = new StreamReader(stream))
        {
            string line;
            while ((line = reader.ReadLine()) is not null)
            {
                lines.Add(line);
            }
        }

        return lines;
    }

    private void StoreTrades(List<Trade> trades)
    {
        using var connection = new SqlConnection("Data Source =.; Initial Catalog = TradingOperation; Integrated Security = True");
        connection.Open();
        using var transaction = connection.BeginTransaction();
        foreach (var trade in trades)
        {
            var command = connection.CreateCommand();
            command.Transaction = transaction;
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.CommandText = "dbo.insert_trade";
            command.Parameters.AddWithValue("@sourceCurrency", trade.SourceCurrency);
            command.Parameters.AddWithValue("@destinationCurrency", trade.DestinationCurrency);
            command.Parameters.AddWithValue("@lots", trade.Lots);
            command.Parameters.AddWithValue("@price", trade.Price);
            command.ExecuteNonQuery();
        }
        transaction.Commit();
        connection.Close();
        Console.WriteLine("INFO: {0} trades processed", trades.Count);
    }
}
