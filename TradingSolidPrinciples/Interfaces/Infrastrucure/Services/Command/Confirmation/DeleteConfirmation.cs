using TradingSolidPrinciples.Interfaces.Infrastrucure.Interfaces;
using TradingSolidPrinciples.Entities;

namespace TradingSolidPrinciples.Interfaces.Infrastrucure.Services.Command.Confirmation;
public class DeleteConfirmation : IDelete<User>
{
    private readonly IDelete<User> _deleteDecorator;

    public DeleteConfirmation(IDelete<User> deleteDecorator)
    {
        _deleteDecorator = deleteDecorator;
    }
    public void Delete(User entity)
    {
        Console.WriteLine("Are You Sure You Want To Delete This Entity? [Y/N]");
        var keyInformation = Console.ReadKey();
        if (keyInformation.Key is ConsoleKey.Y)
        {
            _deleteDecorator.Delete(entity);
        }
    }
}
