using TradingSolidPrinciples.Entities;
namespace TradingSolidPrinciples.Contravariance;
public class EntityEqualityComparer : IEqualityComparer<Entity>
{
    public bool Equals(Entity left, Entity right) => left.ID == right.ID;

}
