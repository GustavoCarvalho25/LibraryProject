namespace Core.Entities;

public class User
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string Email { get; private set; }
    public List<Loan> Loans { get; private set; }
    
    public User() {}
}