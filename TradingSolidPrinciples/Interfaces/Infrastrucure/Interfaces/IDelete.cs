using TradingSolidPrinciples.Entities;

namespace TradingSolidPrinciples.Interfaces.Infrastrucure.Interfaces;
public interface IDelete<TEntity> where TEntity : Entity
{
    void Delete(TEntity entity);
}
