using TradingSolidPrinciples.Entities;
namespace TradingSolidPrinciples.Interfaces.Infrastructure;
public interface IUpdate<TEntity> where TEntity : Entity
{
    void Update(TEntity entity);
}
