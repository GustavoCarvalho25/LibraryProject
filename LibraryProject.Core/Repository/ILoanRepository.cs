using Core.Entities;

namespace Core.Repository;

public interface ILoanRepository : IBaseRepository<Loan>
{ 
    public Task<IEnumerable<Loan>> GetLoansByUserId(Guid userId);
    public Task<Loan?> GetActiveLoanByBookId(Guid bookId);
}