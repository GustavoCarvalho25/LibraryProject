namespace Core.Entities;

public class User : Entity
{
    public string Name { get; private set; }
    public string Email { get; private set; }
    public virtual List<Loan> Loans { get; set; }
    
    protected User() {}
    
    public User(string name, string email) : base()
    {
        Name = name;
        Email = email;
    }
}