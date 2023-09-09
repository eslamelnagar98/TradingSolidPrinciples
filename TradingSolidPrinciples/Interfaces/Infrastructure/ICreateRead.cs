namespace TradingSolidPrinciples.Interfaces.Infrastructure;
public interface ICreateRead<TEntity> where TEntity : class
{
    void Create(TEntity entity);
    TEntity GetById(Guid identity);
    IEnumerable<TEntity> GetALl();
}
