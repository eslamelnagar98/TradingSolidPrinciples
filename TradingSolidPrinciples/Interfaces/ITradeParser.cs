using TradingSolidPrinciples.Entities;
namespace TradingSolidPrinciples.Interfaces;
public interface ITradeParser
{
    IEnumerable<Trade> Parse(IEnumerable<string> lines );
}
