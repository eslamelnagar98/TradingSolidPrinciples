using TradingSolidPrinciples.Entities;
namespace TradingSolidPrinciples.Contravariance;
public class UserEqualityComparer : IEqualityComparer<User>
{
    public bool Equals(User left, User right) => left.ID == right.ID;
}
