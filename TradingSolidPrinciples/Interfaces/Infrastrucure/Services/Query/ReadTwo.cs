using TradingSolidPrinciples.Interfaces.Infrastrucure.Interfaces;
using TradingSolidPrinciples.Entities;

namespace TradingSolidPrinciples.Interfaces.Infrastrucure.Services.Query;
public class ReadTwo : IRead<User>
{
    public IEnumerable<User> GetAll()
    {
        throw new NotImplementedException();
    }

    public User GetById(Guid id)
    {
        return new User(10);
    }
}
