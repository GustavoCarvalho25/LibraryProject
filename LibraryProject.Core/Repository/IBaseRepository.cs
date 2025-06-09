using Core.Entities;

namespace Core.Repository;

public interface IBaseRepository<TEntity> : IDisposable
{ 
    Task<TEntity> Add(TEntity entity);
    
    Task<TEntity> Update(TEntity entity);

    Task<TEntity> Remove(TEntity entity);

    Task<TEntity> GetById(Guid id);
    
    Task<IEnumerable<TEntity>> GetAll();
}