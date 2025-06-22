namespace Application.ViewModels;

public class UserViewModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public int ActiveLoans { get; set; }
}
