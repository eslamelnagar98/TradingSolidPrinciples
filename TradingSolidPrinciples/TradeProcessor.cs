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

    private List<Trade> ParseTrades(List<string> lines)
    {
        var trades = new List<Trade>();
        var lineCount = 1;
        foreach (var line in lines)
        {
            var fields = line.Split(new char[] { ',' });
            if (!ValidateTradeData(fields, lineCount))
            {
                continue;
            }
            var trade = MapTradeDataToTradeRecord(fields);
            trades.Add(trade);
            lineCount++;
        }

        return trades;
    }

    private List<string> ReadTradeData(Stream stream)
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

    private bool ValidateTradeData(string[] fields, int currentLine)
    {
        if (fields.Length != 3)
        {
            LogMessage("WARN: Line {0} malformed. Only {1} field(s) found.", currentLine, fields.Length);
            return false;
        }
        if (fields[0].Length != 6)
        {
            LogMessage("WARN: Trade currencies on line {0} malformed: '{1}'", currentLine, fields[0]);
            return false;
        }

        int tradeAmount;
        if (!int.TryParse(fields[1], out tradeAmount))
        {
            LogMessage("WARN: Trade amount on line {0} not a valid integer: '{1}'", currentLine, fields[1]);
            return false;
        }
        decimal tradePrice;
        if (!decimal.TryParse(fields[2], out tradePrice))
        {
            LogMessage("WARN: Trade price on line {0} not a valid decimal: '{1}'",
            currentLine, fields[2]);
            return false;
        }
        return true;
    }

    private void LogMessage(string message, params object[] args)
    {
        Console.WriteLine(message, args);
    }

    private Trade MapTradeDataToTradeRecord(string[] fields)
    {
        var sourceCurrencyCode = fields[0].Substring(0, 3);
        var destinationCurrencyCode = fields[0].Substring(3, 3);
        var tradeAmount = int.Parse(fields[1]);
        var tradePrice = decimal.Parse(fields[2]);
        var tradeRecord = new Trade
        {
            SourceCurrency = sourceCurrencyCode,
            DestinationCurrency = destinationCurrencyCode,
            Lots = tradeAmount / _lotSize,
            Price = tradePrice
        };
        return tradeRecord;
    }
}
