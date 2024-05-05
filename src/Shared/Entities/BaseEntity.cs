namespace Shared.Entities;

public abstract class BaseEntity
{
    public int Id { get; init; }

    protected BaseEntity(int id)
    {
        Id = id;
    }

    protected BaseEntity()
    {
    }
}
