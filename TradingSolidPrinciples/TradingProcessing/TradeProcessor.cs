using TradingSolidPrinciples.Interfaces;
using TradingSolidPrinciples.TradingProcessing.Interfaces;

namespace TradingSolidPrinciples.TradingProcessing;
public class TradeProcessor : ITradeProcessor
{
    private readonly ITradeDataProvider _tradeDataProvider;
    private readonly ITradeStorage _tradeStorage;
    private readonly ITradeParser _tradeParser;
    public TradeProcessor(ITradeDataProvider tradeDataProvider, ITradeStorage tradeStorage, ITradeParser tradeParser)
    {
        
    }
    public void ProcessTrades()
    {
        var lines = _tradeDataProvider.GetTradeData();
        var trades = _tradeParser.Parse(lines);
        _tradeStorage.Presist(trades);
    }
}
