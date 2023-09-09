using TradingSolidPrinciples.Interfaces.Infrastrucure.Interfaces;
using TradingSolidPrinciples.Entities;

namespace TradingSolidPrinciples.Interfaces.Infrastrucure.Services.Command.Confirmation;
public class CreateConfirmation : ICreate<User>
{
    private readonly ICreate<User> _createDecorator;
    public CreateConfirmation(ICreate<User> createDecorator)
    {
        _createDecorator = createDecorator ?? throw new ArgumentNullException(nameof(createDecorator));
    }
    public void Create(User entity)
    {
        Console.WriteLine("Are You Sure You Want To Create This Entity? [Y/N]");
        var keyInfo = Console.ReadKey();
        if (keyInfo.Key is ConsoleKey.Y)
        {
            _createDecorator.Create(entity);
        }
    }
}
