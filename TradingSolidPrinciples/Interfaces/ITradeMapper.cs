using TradingSolidPrinciples.Entities;
namespace TradingSolidPrinciples.Interfaces;
public interface ITradeMapper
{
    Trade Map(string[] fields);
}
