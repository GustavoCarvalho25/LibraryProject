using System.Linq.Dynamic.Core;
using Core.Entities;
using Core.Repository;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class UserRepository : BaseRepository<User>, IUserRepository
{
    private readonly LibraryDbContext _context;

    public UserRepository(LibraryDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<User?> GetByEmail(string email)
    => await _context.Users
        .AsNoTracking()
        .FirstOrDefaultAsync(u => u.Email == email && !u.IsRemoved);

    public async Task<IEnumerable<User>> GetUsersWithLoans()
    => await _context.Users
        .Where(u => !u.IsRemoved)
        .Include(u => u.Loans)
        .ToListAsync();
}