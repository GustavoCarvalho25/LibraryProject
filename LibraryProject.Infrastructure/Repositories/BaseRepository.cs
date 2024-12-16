using Core.Repositories;

namespace Infrastructure.Repositories;

public class BaseRepository : IBaseRepository
{
    public Task<T> AddAsync<T>(T entity)
    {
        throw new NotImplementedException();
    }

    public Task<int> DeleteAsync<T>(T entity)
    {
        throw new NotImplementedException();
    }

    public Task<int> UpdateAsync<T>(T entity)
    {
        throw new NotImplementedException();
    }

    public Task<T> GetAllAsync<T>()
    {
        throw new NotImplementedException();
    }

    public Task<T> GetByIdAsync<T>(Guid id)
    {
        throw new NotImplementedException();
    }
}