namespace TradingSolidPrinciples.Interfaces;
public interface ITradeDataProvider
{
    IEnumerable<string> GetTradeData();
}
