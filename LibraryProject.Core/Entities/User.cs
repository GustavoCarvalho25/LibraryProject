namespace Core.Entities;

public class User : Entity
{
    public string Name { get; set; }
    public string Email { get; set; }
    public List<Loan> Loans { get; set; }
    
    public User() {}
    
    public User(string name, string email, List<Loan> loans)
    {
        Name = name;
        Email = email;
        Loans = loans;
    }
}