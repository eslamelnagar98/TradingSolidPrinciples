using TradingSolidPrinciples.Entities;
using TradingSolidPrinciples.Interfaces.Infrastrucure.Interfaces;
namespace TradingSolidPrinciples.Interfaces.Infrastrucure.Services;
public class CrudLogging : IRead<User>, IDelete<User>, IUpdate<User>, ICreate<User>
{
    private readonly IRead<User> _readDecorator;
    private readonly ICreate<User> _createDecorator;
    private readonly IDelete<User> _deleteDecorator;
    private readonly IUpdate<User> _updateDecorator;
    private readonly ILogger _logger;
    public CrudLogging(IRead<User> readDecorator,
                       ICreate<User> createDecorator,
                       IDelete<User> deleteDecorator,
                       IUpdate<User> updateDecorator,
                       ILogger logger)
    {
        _readDecorator = readDecorator ?? throw new ArgumentNullException(nameof(readDecorator));
        _createDecorator = createDecorator ?? throw new ArgumentNullException(nameof(createDecorator));
        _deleteDecorator = deleteDecorator ?? throw new ArgumentNullException(nameof(deleteDecorator));
        _updateDecorator = updateDecorator ?? throw new ArgumentNullException(nameof(updateDecorator));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }
    public void Create(User entity)
    {
        _logger.LogInformation("Creating Entity Of Type", typeof(Entity).Name);
        _createDecorator.Create(entity);
        _logger.LogInformation("Entity Created Succsfully For Type ", typeof(Entity).Name);
    }

    public IEnumerable<User> GetAll()
    {
        _logger.LogInformation("GetAll Data Of Type", typeof(Entity).Name);
        return _readDecorator.GetAll();
    }

    public User GetById(Guid id)
    {
        _logger.LogInformation("Get Row Of Type, For Id", typeof(Entity).Name, id);
        return _readDecorator.GetById(id);
    }

    public void Update(User entity)
    {
        _logger.LogInformation("Update Entity Of Type", typeof(Entity).Name);
        _updateDecorator.Update(entity);
        _logger.LogInformation("Update Succesfully Entity Of Type", typeof(Entity).Name);
    }

    public void Delete(User entity)
    {
        _logger.LogInformation("Delete Entity Of Type", typeof(Entity).Name);
        _deleteDecorator.Delete(entity);
        _logger.LogInformation("Delete Succesfully Entity Of Type", typeof(Entity).Name);
    }

}
