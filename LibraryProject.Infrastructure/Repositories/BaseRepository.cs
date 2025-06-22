using Core.Entities;
using Core.Repository;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class BaseRepository<TEntity> : IBaseRepository<TEntity>, IAsyncDisposable where TEntity : Entity
{
    private readonly LibraryDbContext _context;

    public BaseRepository(LibraryDbContext context)
    {
        _context = context;
    }

    public async Task<TEntity> Add(TEntity entity)
    {
        _context.Set<TEntity>().Add(entity);
        await _context.SaveChangesAsync();
        return await Task.FromResult(entity);
    }

    public async Task<TEntity> Update(TEntity entity)
    {
        _context.Set<TEntity>().Update(entity);
        await _context.SaveChangesAsync();
        return await Task.FromResult(entity);
    }

    public async Task<TEntity> Remove(TEntity entity)
    {
        _context.Set<TEntity>().Remove(entity);
        await _context.SaveChangesAsync();
        return await Task.FromResult(entity);
    }

    public async Task<TEntity> GetById(Guid id)
    {
        var entity = await _context.FindAsync<TEntity>(id);
        
        if (entity == null)
            throw new KeyNotFoundException($"Entity with ID {id} not found.");
        
        return entity;
    }

    public async Task<IEnumerable<TEntity>> GetAll()
    {
        return await _context.Set<TEntity>().ToListAsync();
    }

    public async ValueTask DisposeAsync()
    {
        await _context.DisposeAsync();
    }
    
    public void Dispose()
    {
        _context.Dispose();
    }
}