using Core.Entities;

namespace Core.Repository;

public interface IBaseRepository<T> where T : class
{ 
    Task<T> Post(T entity);
}