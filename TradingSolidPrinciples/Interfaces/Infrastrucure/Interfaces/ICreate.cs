using TradingSolidPrinciples.Entities;

namespace TradingSolidPrinciples.Interfaces.Infrastrucure.Interfaces;
public interface ICreate<TEntity> where TEntity : Entity
{
    void Create(TEntity entity);
}
