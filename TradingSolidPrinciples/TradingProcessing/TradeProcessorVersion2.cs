using TradingSolidPrinciples.Interfaces;
namespace TradingSolidPrinciples.TradingProcessing;
public class TradeProcessorVersion2 : TradeProcessor
{
    public TradeProcessorVersion2(ITradeDataProvider tradeDataProvider, ITradeStorage tradeStorage, ITradeParser tradeParser)
        : base(tradeDataProvider, tradeStorage, tradeParser) { }
}
