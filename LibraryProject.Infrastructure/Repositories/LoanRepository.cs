using Core.Entities;
using Core.Repository;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class LoanRepository : BaseRepository<Loan>, ILoanRepository
{
    private readonly LibraryDbContext _context;

    public LoanRepository(LibraryDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Loan>> GetLoansByUserId(Guid userId)
    => await _context.Loans
        .Where(l => l.CustomerId == userId)
        .ToListAsync();
}