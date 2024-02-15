using System.Linq.Expressions;

using Microsoft.EntityFrameworkCore;
using VideoSharingPlatform.Core.Interfaces;

namespace VideoSharingPlatform.Persistent.Data;

public class EfRepository<T> : IReadRepository<T>, IRepository<T>
    where T : class, IAggregateRoot
{
    private readonly ApplicationDbContext _context;

    public EfRepository(ApplicationDbContext context) => _context = context;

    public virtual Task<T?> GetByIdAsync<TKey>(TKey id, CancellationToken cancellationToken = default)
        where TKey : notnull, IComparable => _context.Set<T>().FindAsync(id, cancellationToken).AsTask();

    public virtual Task<List<T>> ListAsync(CancellationToken cancellationToken = default) =>
        _context.Set<T>().ToListAsync(cancellationToken);

    public virtual Task<List<T>> GetPageAsync<TKey>(int pageNumber,
        int pageSize,
        Func<T, TKey> keySelector,
        CancellationToken cancellationToken = default
    ) =>
        _context.Set<T>()
            .OrderBy(keySelector)
            .AsQueryable()
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

    public virtual Task<int> CountAsync(CancellationToken cancellationToken = default) => 
        _context.Set<T>().CountAsync(cancellationToken);

    public virtual Task<int> CountAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default) =>
        _context.Set<T>().CountAsync(predicate, cancellationToken);

    public virtual Task<bool> AnyAsync(CancellationToken cancellationToken = default) =>
        _context.Set<T>().AnyAsync(cancellationToken);

    public virtual Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default) =>
        _context.Set<T>().AnyAsync(predicate, cancellationToken);

    public async Task<T> AddAsync(T entity, CancellationToken cancellationToken = default) {
        _context.Set<T>().Add(entity);
        await SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task UpdateAsync(T entity, CancellationToken cancellationToken = default) {
        _context.Set<T>().Update(entity);
        await SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(T entity, CancellationToken cancellationToken = default) {
        _context.Set<T>().Remove(entity);
        await SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default) {
        _context.Set<T>().RemoveRange(entities);
        await SaveChangesAsync(cancellationToken);
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
        _context.SaveChangesAsync(cancellationToken);
}