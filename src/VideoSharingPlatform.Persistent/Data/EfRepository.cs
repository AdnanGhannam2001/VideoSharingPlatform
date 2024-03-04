using System.Linq.Expressions;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

using VideoSharingPlatform.Core.Common;

using VideoSharingPlatform.Core.Interfaces;

namespace VideoSharingPlatform.Persistent.Data;

public class EfRepository<T> : IReadRepository<T>, IRepository<T>
    where T : class, IAggregateRoot
{
    private readonly ApplicationDbContext _context;

    public EfRepository(ApplicationDbContext context) => _context = context;

    protected IQueryable<T> GetQueryable() => _context.Set<T>();

    public Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
        => _context.Database.BeginTransactionAsync(cancellationToken);

    public virtual async Task<T?> GetByIdAsync<TKey>(TKey id, CancellationToken cancellationToken = default)
        where TKey : notnull, IComparable
            => await _context.Set<T>().FindAsync(id, cancellationToken);

    public virtual Task<List<T>> ListAsync(CancellationToken cancellationToken = default)
        => _context.Set<T>().AsNoTracking().ToListAsync(cancellationToken);

    public virtual async Task<Page<T>> GetPageAsync<TKey>(int pageNumber,
        int pageSize,
        Func<T, TKey>? keySelector = null,
        bool desc = false,
        CancellationToken cancellationToken = default)
    {
        // TODO: Fix this (Ordering)
        var query = GetQueryable()
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .AsNoTracking();

        var total = await CountAsync(cancellationToken);

        if (keySelector is not null) {
            var items = desc
                ? query.OrderByDescending(keySelector)
                : query.OrderBy(keySelector);
            
            return new (items.ToList(), total);
        } else {
            var items = desc
                ? query.OrderDescending()
                : query.Order();

            return new (items.ToList(), total);
        }
    }

    public virtual Task<int> CountAsync(CancellationToken cancellationToken = default) => 
        _context.Set<T>().AsNoTracking().CountAsync(cancellationToken);

    public virtual Task<int> CountAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default) =>
        _context.Set<T>().AsNoTracking().CountAsync(predicate, cancellationToken);

    public virtual Task<bool> AnyAsync(CancellationToken cancellationToken = default) =>
        _context.Set<T>().AsNoTracking().AnyAsync(cancellationToken);

    public virtual Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default) =>
        _context.Set<T>().AsNoTracking().AnyAsync(predicate, cancellationToken);

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