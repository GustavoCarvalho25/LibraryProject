namespace Core.Entities;

public abstract class Entity
{
    public Guid Id { get; set; }
    public bool IsRemoved { get; set; } = false;
    
    public Entity()
    {
        Id = Guid.NewGuid();
    }
}