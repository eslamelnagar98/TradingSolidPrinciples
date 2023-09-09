using TradingSolidPrinciples.Entities;

namespace TradingSolidPrinciples.Covariance;
public class UserRepository : IEntityRepository<User>
{
    public User GetByID(Guid id)
    {
        return new User(20);
    }
}
