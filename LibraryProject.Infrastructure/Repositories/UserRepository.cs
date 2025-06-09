using Core.Entities;
using Core.Repository;
using Infrastructure.Persistence;

namespace Infrastructure.Repositories;

public class UserRepository : BaseRepository<User>, IUserRepository
{
    private readonly LibraryDbContext _context;

    public UserRepository(LibraryDbContext context) : base(context)
    {
        _context = context;
    }

    public Task<User> GetByEmail(string email)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<User>> GetUsersWithLoans()
    {
        throw new NotImplementedException();
    }
}