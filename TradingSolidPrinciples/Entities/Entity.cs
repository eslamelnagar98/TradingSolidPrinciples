namespace TradingSolidPrinciples.Entities;
public class Entity
{
    private decimal _flateRate;
    public Entity(decimal flateRate)
    {
        if (flateRate <= decimal.Zero)
        {
            throw new ArgumentOutOfRangeException(nameof(flateRate));
        }

        _flateRate = flateRate;
    }
    public Guid Id { get; set; }
    public string Name { get; set; }

    protected decimal FlateRate
    {
        get
        {
            return _flateRate;
        }
        set
        {
            if (value <= decimal.Zero)
            {
                throw new ArgumentOutOfRangeException(nameof(FlateRate));
            }
            _flateRate = value;
        }
    }
    public virtual void Add(Entity entity, int length)
    {
        if (entity is null)
        {
            throw new ArgumentNullException(nameof(entity));
        }
    }
    public virtual List<string> GetAllData(int lengthOfOutPutResult)
    {
        if (lengthOfOutPutResult is < 5)
        {
            throw new ArgumentOutOfRangeException(nameof(lengthOfOutPutResult));
        }

        var data = default(List<string>);
        if (data is null)
        {
            throw new ArgumentNullException(nameof(data));
        }

        if (data.Count > 10)
        {
            throw new IndexOutOfRangeException(nameof(data));
        }

        return data;
    }

    protected List<string> GetDataFromMemory()
    {
        return Enumerable.Range(0, 10)
            .Select(x => x.ToString())
            .ToList();
    }
}
