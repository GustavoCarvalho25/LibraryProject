namespace Core.Entities;

public class Entity
{
    public Guid Id { get; set; }
    
    public Entity()
    {
        Id = Guid.NewGuid();
    }
}