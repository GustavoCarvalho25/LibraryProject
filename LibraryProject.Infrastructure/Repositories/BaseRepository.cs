using System.Linq.Expressions;
using Core.Entities;
using Core.Repository;
using Core.Shared;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using DynamicLinq = System.Linq.Dynamic.Core;

namespace Infrastructure.Repositories;

public class BaseRepository<TEntity> : IBaseRepository<TEntity>, IAsyncDisposable where TEntity : Entity
{
    protected readonly LibraryDbContext _context;
    protected readonly DbSet<TEntity> _dbSet;

    public BaseRepository(LibraryDbContext context)
    {
        _context = context;
        _dbSet = _context.Set<TEntity>();
    }

    public virtual async Task<TEntity> Add(TEntity entity)
    {
        _dbSet.Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public virtual async Task<bool> Update(TEntity entity)
    {
        _dbSet.Update(entity);
        return await _context.SaveChangesAsync() > 0;
    }

    public virtual async Task<bool> Remove(TEntity entity)
    {
        _dbSet.Remove(entity);
        return await _context.SaveChangesAsync() > 0;
    }

    public virtual async Task<TEntity?> GetById(Guid id)
    {
        var entity = await _dbSet.FindAsync(id);
        return entity is { IsRemoved: false } ? entity : null;
    }

    public virtual async Task<IEnumerable<TEntity>> GetAll()
    {
        return await _dbSet.Where(e => !e.IsRemoved).ToListAsync();
    }
    
    public virtual async Task<PagedResult<TEntity>> GetPagedAsync(
        QueryOptions options,
        Expression<Func<TEntity, bool>>? filter = null,
        CancellationToken cancellationToken = default)
    {
        var query = _dbSet.AsQueryable()
            .Where(e => e.IsRemoved == false);
        
        if (filter != null)
            query = query.Where(filter);
        
        var totalCount = await query.CountAsync(cancellationToken);
        
        if (!string.IsNullOrWhiteSpace(options.OrderBy))
        {
            try
            {
                var propertyExists = typeof(TEntity).GetProperties()
                    .Any(p => string.Equals(p.Name, options.OrderBy, StringComparison.OrdinalIgnoreCase));
                
                if (propertyExists)
                {
                    query = options.OrderByDescending
                        ? DynamicLinq.DynamicQueryableExtensions.OrderBy(query, $"{options.OrderBy} DESC")
                        : DynamicLinq.DynamicQueryableExtensions.OrderBy(query, options.OrderBy);
                }
                else
                {
                    query = options.OrderByDescending
                        ? query.OrderByDescending(e => e.Id)
                        : query.OrderBy(e => e.Id);
                }
            }
            catch
            {
                query = options.OrderByDescending
                    ? query.OrderByDescending(e => e.Id)
                    : query.OrderBy(e => e.Id);
            }
        }
        else
        {
            query = query.OrderBy(e => e.Id);
        }
        
        var items = await query
            .Skip((options.PageNumber - 1) * options.PageSize)
            .Take(options.PageSize)
            .ToListAsync(cancellationToken);
        
        return new PagedResult<TEntity>(items, totalCount, options.PageNumber, options.PageSize);
    }

    public async ValueTask DisposeAsync()
    {
        await _context.DisposeAsync();
    }
    
    public void Dispose()
    {
        _context?.Dispose();
    }
}