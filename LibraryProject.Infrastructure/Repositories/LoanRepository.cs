using Core.Entities;
using Core.Repository;
using Infrastructure.Persistence;

namespace Infrastructure.Repositories;

public class LoanRepository : BaseRepository<Loan>, ILoanRepository
{
    private readonly LibraryDbContext _context;

    public LoanRepository(LibraryDbContext context) : base(context)
    {
        _context = context;
    }

    public Task<IEnumerable<Loan>> GetLoansByUserId(Guid userId)
    {
        throw new NotImplementedException();
    }
}