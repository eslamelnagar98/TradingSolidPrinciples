namespace TradingSolidPrinciples.Entities;
public class User : Entity
{
    public User(decimal flateRate)
        : base(flateRate)
    {

    }
    public string EmailAddress { get; private set; }
    public DateTime DateOfBirth { get; private set; }
    public new decimal FlateRate
    {
        get
        {
            return base.FlateRate;
        }

        set
        {
            base.FlateRate = value;
        }
    }

    public override void Add(Entity entity, int length)
    {
        if (entity is null)
        {
            throw new ArgumentNullException(nameof(entity));
        }

        if (length < 10)
        {
            throw new IndexOutOfRangeException(nameof(length));
        }
    }

    public override List<string> GetAllData(int lengthOfOutPutResult)
    {
        var data = default(List<string>);
        if (data is null)
        {
            throw new ArgumentNullException(nameof(data));
        }
        if (data?.Count > 10)
        {
            throw new IndexOutOfRangeException(nameof(data));
        }

        return data;
    }
}
