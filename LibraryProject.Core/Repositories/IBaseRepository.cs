namespace Core.Repositories;

public interface IBaseRepository
{
    public Task<T> AddAsync<T>(T entity); 
    public Task<int> DeleteAsync<T>(T entity);
    public Task<int> UpdateAsync<T>(T entity);
    public Task<T> GetAllAsync<T>();
    public Task<T> GetByIdAsync<T>(Guid id);
}