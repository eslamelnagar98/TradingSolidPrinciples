using TradingSolidPrinciples.Entities;
namespace TradingSolidPrinciples.Interfaces.Infrastructure;
public class UpdateConfirmation : IUpdate<Entity>
{
    private readonly IUpdate<Entity> _updateDecorator;
    public UpdateConfirmation(IUpdate<Entity> updateDecorator)
    {
        _updateDecorator = updateDecorator;
    }
    public void Update(Entity entity)
    {
        Console.WriteLine("Are you sure you want to Update the entity? [y/N]");
        var keyInfo = Console.ReadKey();
        if (keyInfo.Key == ConsoleKey.Y)
        {
            _updateDecorator.Update(entity);
        }
    }
}
