using TradingSolidPrinciples.Entities;
namespace TradingSolidPrinciples.Interfaces.Infrastructure;
public class CrudLogging : ICreateReadUpdateDelete<Entity>
{
    private readonly ICreateReadUpdateDelete<Entity> _createReadUpdateDelete;
    private readonly ILogger _logger;
    public CrudLogging(ICreateReadUpdateDelete<Entity> createReadUpdateDelete, ILogger logger)
    {
        _createReadUpdateDelete = createReadUpdateDelete;
        _logger = logger;
    }
    public void Create(Entity entity)
    {
        _logger.LogInformation("Creating entity of type {0}", typeof(Entity).Name);
        _createReadUpdateDelete.Create(entity);
    }

    public void Delete(Entity entity)
    {
        _logger.LogInformation("Deleting entity of type {0}", typeof(Entity).Name);
        _createReadUpdateDelete.Delete(entity);
    }

    public IEnumerable<Entity> GetALl()
    {
        _logger.LogInformation("Reading all entities of type {0}", typeof(Entity).Name);
        return _createReadUpdateDelete.GetALl();
    }

    public Entity GetById(Guid identity)
    {
        _logger.LogInformation("Reading entity of type {0} with identity {1}", typeof(Entity).Name, identity);
        return _createReadUpdateDelete.GetById(identity);
    }

    public void Update(Entity entity)
    {
        _logger.LogInformation("Updating entity of type {0}", typeof(Entity).Name);
        _createReadUpdateDelete.Update(entity);
    }
}
