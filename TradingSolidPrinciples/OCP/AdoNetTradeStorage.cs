using SD_PD_IDA_46.SRP;
using SD_PD_IDA_46.SRP.Models;
using System.Data.SqlClient;

namespace SD_PD_IDA_46.OCP;

public sealed class AdoNetTradeStorage : ITradeStorage
{
    public void Persist(IEnumerable<Trade> trades)
    {
        using var connection = new SqlConnection("Data Source=.; Initial Catalog=TradingOperation; Integrated Security=True");
        connection.Open();
        using var transaction = connection.BeginTransaction();
        foreach (var trade in trades)
        {
            var command = connection.CreateCommand();
            command.Transaction = transaction;
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.CommandText = "dbo.insert_trade";
            command.Parameters.AddWithValue("@sourceCurrency", trade.SourceCurrency);
            command.Parameters.AddWithValue("@destinationCurrency", trade.TargetCurrency);
            command.Parameters.AddWithValue("@lots", trade.Lots);
            command.Parameters.AddWithValue("@price", trade.Price);
            command.ExecuteNonQuery();
        }
        transaction.Commit();
        TradeLogger.LogInfo("Trades saved to SQL database", trades.Count());
    }
}
