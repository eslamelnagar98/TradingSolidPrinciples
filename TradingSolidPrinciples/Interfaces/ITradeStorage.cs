using TradingSolidPrinciples.Entities;
namespace TradingSolidPrinciples.Interfaces;
public interface ITradeStorage
{
    void Presist(IEnumerable<Trade> trades);
}
