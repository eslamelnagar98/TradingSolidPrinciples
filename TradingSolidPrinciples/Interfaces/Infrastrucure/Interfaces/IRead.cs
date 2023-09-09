using TradingSolidPrinciples.Entities;

namespace TradingSolidPrinciples.Interfaces.Infrastrucure.Interfaces;
public interface IRead<TEntity> where TEntity : Entity
{
    TEntity GetById(Guid id);
    IEnumerable<TEntity> GetAll();
}
