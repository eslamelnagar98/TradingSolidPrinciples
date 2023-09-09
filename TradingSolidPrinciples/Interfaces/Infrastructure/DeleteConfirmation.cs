using TradingSolidPrinciples.Entities;
namespace TradingSolidPrinciples.Interfaces.Infrastructure;
public class DeleteConfirmation : IDelete<Entity>
{
    private readonly IDelete<Entity> _deleteDecorator;
    public DeleteConfirmation(IDelete<Entity> deleteDecorator)
    {
        _deleteDecorator = deleteDecorator;
    }
    public void Delete(Entity entity)
    {
        Console.WriteLine("Are you sure you want to delete the entity? [y/N]");
        var keyInfo = Console.ReadKey();
        if (keyInfo.Key == ConsoleKey.Y)
        {
            _deleteDecorator.Delete(entity);
        }
    }

}
