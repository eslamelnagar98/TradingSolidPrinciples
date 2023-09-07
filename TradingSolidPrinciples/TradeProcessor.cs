using System.Data.SqlClient;
using TradingSolidPrinciples.Entities;
using TradingSolidPrinciples.Interfaces;

namespace TradingSolidPrinciples;
public class TradeProcessor
{
    private static readonly float _lotSize = 100000f;
    private readonly ITradeDataProvider _tradeDataProvider;
    private readonly ITradeStorage _tradeStorage;
    private readonly ITradeParser _tradeParser;

    public TradeProcessor(ITradeDataProvider tradeDataProvider, ITradeStorage tradeStorage, ITradeParser tradeParser)
    {
        _tradeDataProvider = tradeDataProvider;
        _tradeStorage = tradeStorage;
        _tradeParser = tradeParser;
    }
    public void ProcessTrades()
    {
        var lines = _tradeDataProvider.GetTradeData();
        var trades = _tradeParser.Parse(lines);
        _tradeStorage.Presist(trades);
    }
}
