namespace TradingSolidPrinciples.Interfaces.Infrastructure;
public interface ICreateReadUpdateDelete<TEntity> where TEntity : class
{
    void Create(TEntity entity);
    TEntity GetById(Guid identity);
    IEnumerable<TEntity> GetALl();
    void Update(TEntity entity);
}
