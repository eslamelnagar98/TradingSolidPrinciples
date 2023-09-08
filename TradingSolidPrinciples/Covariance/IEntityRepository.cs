using TradingSolidPrinciples.Entities;

namespace TradingSolidPrinciples.Covariance;
public interface IEntityRepository<out TEntity> where TEntity : Entity
{
    TEntity GetByID(Guid id);
}
