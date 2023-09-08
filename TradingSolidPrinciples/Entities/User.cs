namespace TradingSolidPrinciples.Entities;
public class User : Entity
{
    public string EmailAddress { get; private set; }
    public DateTime DateOfBirth { get; private set; }
}
