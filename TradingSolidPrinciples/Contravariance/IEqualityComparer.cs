using TradingSolidPrinciples.Entities;
namespace TradingSolidPrinciples.Contravariance;
public interface IEqualityComparer<in TEntity> where TEntity : Entity
{
    bool Equals(TEntity left, TEntity right);
}
