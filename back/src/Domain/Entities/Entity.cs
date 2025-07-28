namespace Domain.Entities;

public abstract class Entity
{
    public Guid Id { get; protected set; } 

    protected Entity()
    {
        Id = Guid.NewGuid(); 
    }

    public override bool Equals(object obj)
    {
        if (obj == null || obj.GetType() != GetType())
        {
            return false;
        }

        var entity = (Entity)obj;
        return Id == entity.Id;
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    public static bool operator ==(Entity left, Entity right)
    {
        if (ReferenceEquals(left, null) && ReferenceEquals(right, null))
        {
            return true;
        }

        if (ReferenceEquals(left, null) || ReferenceEquals(right, null))
        {
            return false;
        }

        return left.Equals(right);
    }

    public static bool operator !=(Entity left, Entity right)
    {
        return !(left == right);
    }
}
