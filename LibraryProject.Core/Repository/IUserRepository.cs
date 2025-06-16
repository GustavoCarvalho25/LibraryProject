using Core.Entities;

namespace Core.Repository;

public interface IUserRepository : IBaseRepository<User>
{ 
    Task<User?> GetByEmail(string email);
    Task<IEnumerable<User>> GetUsersWithLoans();
}