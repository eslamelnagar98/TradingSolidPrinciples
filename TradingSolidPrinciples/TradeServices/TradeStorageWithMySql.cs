using TradingSolidPrinciples.Entities;
using TradingSolidPrinciples.Interfaces;

namespace TradingSolidPrinciples.TradeServices;

public class TradeStorageWithMySql : ITradeStorage
{
    private readonly ILogger _logger;

    public TradeStorageWithMySql(ILogger logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }
    
    public void Presist(IEnumerable<Trade> trades)
    {
        _logger.LogInformation("Persisting trades to MySQL...", trades?.Count());
    }
}
