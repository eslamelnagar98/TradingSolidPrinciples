using TradingSolidPrinciples.Entities;
namespace TradingSolidPrinciples.Interfaces.Infrastructure;
public class CrudLogging : ICreateReadUpdate<Entity>, IDelete<Entity>
{
    private readonly ICreateReadUpdate<Entity> _createReadUpdateDelete;
    private readonly ILogger _logger;
    private readonly IDelete<Entity> _deleteDecorator;
    public CrudLogging(ICreateReadUpdate<Entity> createReadUpdateDelete, ILogger logger, IDelete<Entity> deleteDecorator)
    {
        _createReadUpdateDelete = createReadUpdateDelete;
        _logger = logger;
        _deleteDecorator = deleteDecorator;
    }
    public void Create(Entity entity)
    {
        _logger.LogInformation("Creating entity of type {0}", typeof(Entity).Name);
        _createReadUpdateDelete.Create(entity);
    }

    public void Delete(Entity entity)
    {
        _logger.LogInformation("Deleting entity of type {0}", typeof(Entity).Name);
        _deleteDecorator.Delete(entity);
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
