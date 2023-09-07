using TradingSolidPrinciples.Interfaces;
namespace TradingSolidPrinciples.TradingProcessing;
public class TradeProcessorVersion2 : TradeProcessorAbstract
{
    public TradeProcessorVersion2(ITradeDataProvider tradeDataProvider, ITradeStorage tradeStorage, ITradeParser tradeParser)
        : base(tradeDataProvider, tradeStorage, tradeParser) { }

    public override void ProcessTrades()
    {
        //Another Implementaion
    }
}
