using TradingSolidPrinciples.Entities;
using TradingSolidPrinciples.Interfaces;

namespace TradingSolidPrinciples.TradeServices;
public class SimpleTradeMapper : ITradeMapper
{
    private static readonly float _lotSize = 100000f;
    public Trade Map(string[] fields)
    {
        var sourceCurrencyCode = fields[0].Substring(0, 3);
        var destinationCurrencyCode = fields[0].Substring(3, 3);
        var tradeAmount = int.Parse(fields[1]);
        var tradePrice = decimal.Parse(fields[2]);
        var tradeRecord = new Trade
        {
            SourceCurrency = sourceCurrencyCode,
            DestinationCurrency = destinationCurrencyCode,
            Lots = tradeAmount / _lotSize,
            Price = tradePrice
        };
        return tradeRecord;
    }
}
