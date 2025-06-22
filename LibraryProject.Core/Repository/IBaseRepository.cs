using System.Linq.Expressions;
using Core.Entities;
using Core.Shared;

namespace Core.Repository;

public interface IBaseRepository<TEntity> : IDisposable where TEntity : Entity
{
    Task<TEntity?> GetById(Guid id);
    Task<IEnumerable<TEntity>> GetAll();
    Task<TEntity> Add(TEntity entity);
    Task<bool> Update(TEntity entity);
    Task<bool> Remove(TEntity entity);
    
    Task<Core.Shared.PagedResult<TEntity>> GetPagedAsync(
        Core.Shared.QueryOptions options,
        Expression<Func<TEntity, bool>>? filter = null,
        CancellationToken cancellationToken = default);
}