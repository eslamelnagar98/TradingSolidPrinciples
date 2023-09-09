using TradingSolidPrinciples.Entities;
namespace TradingSolidPrinciples.Interfaces.Infrastructure;
public interface IDelete<TEntity> where TEntity : Entity
{
    void Delete(TEntity entity);
}
