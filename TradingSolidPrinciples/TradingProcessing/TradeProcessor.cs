using TradingSolidPrinciples.Interfaces;
namespace TradingSolidPrinciples.TradingProcessing;
public class TradeProcessor : TradeProcessorAbstract
{
    public TradeProcessor(ITradeDataProvider tradeDataProvider, ITradeStorage tradeStorage, ITradeParser tradeParser)
        : base(tradeDataProvider, tradeStorage, tradeParser) { }
    public override void ProcessTrades()
    {
        var lines = _tradeDataProvider.GetTradeData();
        var trades = _tradeParser.Parse(lines);
        _tradeStorage.Presist(trades);
    }
}
