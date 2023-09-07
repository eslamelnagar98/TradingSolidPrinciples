using System.Data.SqlClient;
using TradingSolidPrinciples.Entities;
using TradingSolidPrinciples.Interfaces;
namespace TradingSolidPrinciples.TradeServices;
public class AdoNetTradeStorage : ITradeStorage
{
    private readonly ILogger _logger;
    public AdoNetTradeStorage(ILogger logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }
    public void Presist(IEnumerable<Trade> trades)
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
        _logger.LogInformation("Trades processed", trades.Count());
    }
}
