using TradingSolidPrinciples.Entities;

namespace TradingSolidPrinciples.Interfaces.Infrastrucure.Interfaces;
public interface IUpdate<TEntity> where TEntity : Entity
{
    void Update(TEntity entity);
}
