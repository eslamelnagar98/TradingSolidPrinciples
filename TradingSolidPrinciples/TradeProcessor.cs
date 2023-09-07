using TradingSolidPrinciples.Interfaces;
namespace TradingSolidPrinciples;
public class TradeProcessor
{
    private readonly ITradeDataProvider _tradeDataProvider;
    private readonly ITradeStorage _tradeStorage;
    private readonly ITradeParser _tradeParser;

    public TradeProcessor(ITradeDataProvider tradeDataProvider, ITradeStorage tradeStorage, ITradeParser tradeParser)
    {
        _tradeDataProvider = tradeDataProvider ?? throw new ArgumentNullException(nameof(tradeDataProvider));
        _tradeParser = tradeParser ?? throw new ArgumentNullException(nameof(tradeParser));
        _tradeStorage = tradeStorage ?? throw new ArgumentNullException(nameof(tradeStorage));
    }
    public void ProcessTrades()
    {
        var lines = _tradeDataProvider.GetTradeData();
        var trades = _tradeParser.Parse(lines);
        _tradeStorage.Presist(trades);
    }
}
