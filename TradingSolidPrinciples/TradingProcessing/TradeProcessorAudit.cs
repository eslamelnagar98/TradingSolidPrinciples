using TradingSolidPrinciples.Interfaces;
namespace TradingSolidPrinciples.TradingProcessing;
public class TradeProcessorAudit
{
    private readonly ITradeDataProvider _tradeDataProvider;
    private readonly ITradeStorage _tradeStorage;
    private readonly ITradeParser _tradeParser;
    public TradeProcessorAudit(ITradeDataProvider tradeDataProvider, ITradeStorage tradeStorage, ITradeParser tradeParser)
    {
        _tradeDataProvider = tradeDataProvider ?? throw new ArgumentNullException(nameof(tradeDataProvider));
        _tradeParser = tradeParser ?? throw new ArgumentNullException(nameof(tradeParser));
        _tradeStorage = tradeStorage ?? throw new ArgumentNullException(nameof(tradeStorage));
    }

    public void ProcessTrades()
    {
       // Implement It Here 
    }

    private void AuditTradeData()
    {
        
    }

    private void ListErors()
    {
        
    }
}