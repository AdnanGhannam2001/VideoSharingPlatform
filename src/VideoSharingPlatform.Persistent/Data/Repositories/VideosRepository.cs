using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

using VideoSharingPlatform.Core.Common;
using VideoSharingPlatform.Core.Entities.VideoAggregate;

namespace VideoSharingPlatform.Persistent.Data.Repositories;

public class VideosRepository : EfRepository<Video>
{
    public VideosRepository(ApplicationDbContext context) : base(context) { }

    public override Task<Video?> GetByIdAsync<TKey>(TKey id, CancellationToken cancellationToken = default) {
        return GetQueryable()
            .Include(x => x.User)
            .FirstOrDefaultAsync(x => x.Id.Equals(id));
    }

    public async Task<Page<Video>> GetPageWithUserAsync<TKey>(int pageNumber,
        int pageSize,
        Func<Video, TKey>? keySelector = null,
        bool desc = false,
        CancellationToken cancellationToken = default)
    {
        var result = keySelector switch
        {
            not null => desc
                ? GetQueryable().OrderByDescending(keySelector).AsQueryable()
                : GetQueryable().OrderBy(keySelector).AsQueryable(),
            _ => GetQueryable()
        };

        var items = await result
            .Include(x => x.User)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        var total = await CountAsync(cancellationToken);

        return new (items, total);
    }

    public async Task<Page<Comment>> GetCommentsAsync(string id,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default) {
        var items = await GetQueryable()
            .Where(x => x.Id.Equals(id))
            .SelectMany(x => x.Comments)
            .Include(x => x.User)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        var total = await GetCommentsCountAsync(id, cancellationToken);

        return new (items, total);
    }

    public Task<int> GetCommentsCountAsync(string id, CancellationToken cancellationToken = default) {
        return GetQueryable()
            .Where(x => x.Id.Equals(id))
            .SelectMany(x => x.Comments)
            .CountAsync(cancellationToken);
    }

    public Task<Video?> GetWithCommentAsync(string id, string commentId, CancellationToken cancellationToken = default) {
        return GetQueryable()
            .Where(x => x.Id.Equals(id))
            .Include(x => x.Comments.Where(c => c.Id.Equals(commentId)))
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<Page<Reaction>> GetReactionsAsync(string id, CancellationToken cancellationToken = default) {
        var items = await GetQueryable()
            .Where(x => x.Id.Equals(id))
            .SelectMany(x => x.Reactions)
            .ToListAsync(cancellationToken);

        var total = await GetReactionsCountAsync(id, null, cancellationToken);

        return new (items, total);
    }

    public Task<int> GetReactionsCountAsync(string id, Expression<Func<Reaction, bool>>? predicate, CancellationToken cancellationToken = default) {
        return GetQueryable()
            .Where(x => x.Id.Equals(id))
            .SelectMany(x => x.Reactions)
            .Where(predicate ?? (x => true))
            .CountAsync(cancellationToken);
    }

    public Task<Video?> GetWithReactionAsync(string id, string userId, CancellationToken cancellationToken = default) {
        return GetQueryable()
            .Where(x => x.Id.Equals(id))
            .Include(x => x.Reactions.Where(r => r.UserId.Equals(userId)))
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<Page<Reaction>> GetReactionsOnCommentAsync(string id, string commentId, CancellationToken cancellationToken = default) {
        var items = await GetQueryable()
            .Where(x => x.Id.Equals(id))
            .SelectMany(x => x.Comments)
            .Where(x => x.Id.Equals(commentId))
            .SelectMany(x => x.Reactions)
            .ToListAsync(cancellationToken);

        var total = await GetReactionsCountOnCommentAsync(id, commentId, null, cancellationToken);

        return new (items, total);
    }

    public Task<int> GetReactionsCountOnCommentAsync(string id, string commentId, Expression<Func<Reaction, bool>>? predicate, CancellationToken cancellationToken = default) {
        return GetQueryable()
            .Where(x => x.Id.Equals(id))
            .SelectMany(x => x.Comments)
            .Where(x => x.Equals(commentId))
            .SelectMany(x => x.Reactions)
            .Where(predicate ?? (x => true))
            .CountAsync(cancellationToken);
    }

    public Task<Video?> GetCommentWithReactionAsync(string id, string commentId, string userId, CancellationToken cancellationToken = default) {
        return GetQueryable()
            .Where(x => x.Id.Equals(id))
            .Include(x => x.Comments.Where(c => c.Id.Equals(commentId)))
                .ThenInclude(x => x.Reactions.Where(r => r.UserId.Equals(userId)))
            .FirstOrDefaultAsync(cancellationToken);
    }
}