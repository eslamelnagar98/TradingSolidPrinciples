using TradingSolidPrinciples.Interfaces.Infrastrucure.Interfaces;
using TradingSolidPrinciples.Entities;

namespace TradingSolidPrinciples.Interfaces.Infrastrucure.Services.Command.Confirmation;
public class UpdateConfirmation : IUpdate<User>
{
    private readonly IUpdate<User> _updateDecorator;
    public UpdateConfirmation(IUpdate<User> updateDecorator)
    {
        _updateDecorator = updateDecorator ?? throw new ArgumentNullException(nameof(updateDecorator));
    }
    public void Update(User entity)
    {
        Console.WriteLine("Are You Sure You Want To Update This Entity? [Y/N]");
        var keyInformation = Console.ReadKey();
        if (keyInformation.Key is ConsoleKey.Y)
        {
            _updateDecorator.Update(entity);
        }
    }
}
