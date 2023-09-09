using TradingSolidPrinciples.Entities;
namespace TradingSolidPrinciples.Interfaces.Infrastructure;
public class DeleteConfirmation : ICreateReadUpdateDelete<Entity>
{
    private readonly ICreateReadUpdateDelete<Entity> _createReadUpdateDelete;
    public DeleteConfirmation(ICreateReadUpdateDelete<Entity> createReadUpdateDelete)
    {
        _createReadUpdateDelete = createReadUpdateDelete;
    }
    public void Create(Entity entity)
    {
        _createReadUpdateDelete.Create(entity);
    }
    public IEnumerable<Entity> GetALl()
    {
        return _createReadUpdateDelete.GetALl();
    }

    public Entity GetById(Guid identity)
    {
        return _createReadUpdateDelete.GetById(identity);
    }

    public void Update(Entity entity)
    {
        _createReadUpdateDelete.Update(entity);
    }

    public void Delete(Entity entity)
    {
        Console.WriteLine("Are you sure you want to delete the entity? [y/N]");
        var keyInfo = Console.ReadKey();
        if (keyInfo.Key == ConsoleKey.Y)
        {
            _createReadUpdateDelete.Delete(entity);
        }
    }

}
