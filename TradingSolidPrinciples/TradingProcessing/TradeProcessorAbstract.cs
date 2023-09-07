using TradingSolidPrinciples.Interfaces;
namespace TradingSolidPrinciples.TradingProcessing;
public abstract class TradeProcessorAbstract
{
    protected readonly ITradeDataProvider _tradeDataProvider;
    protected readonly ITradeStorage _tradeStorage;
    protected readonly ITradeParser _tradeParser;

    protected TradeProcessorAbstract(ITradeDataProvider tradeDataProvider, ITradeStorage tradeStorage, ITradeParser tradeParser)
    {
        _tradeDataProvider = tradeDataProvider ?? throw new ArgumentNullException(nameof(tradeDataProvider));
        _tradeParser = tradeParser ?? throw new ArgumentNullException(nameof(tradeParser));
        _tradeStorage = tradeStorage ?? throw new ArgumentNullException(nameof(tradeStorage));
    }
    public abstract void ProcessTrades();
}
