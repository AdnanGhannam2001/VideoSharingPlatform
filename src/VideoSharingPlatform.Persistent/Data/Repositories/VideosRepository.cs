using System.Linq.Expressions;

using Microsoft.EntityFrameworkCore;
using VideoSharingPlatform.Core.Entities.VideoAggregate;
using VideoSharingPlatform.Core.Enums;

namespace VideoSharingPlatform.Persistent.Data.Repositories;

public class VideosRepository : EfRepository<Video>
{
    public VideosRepository(ApplicationDbContext context) : base(context) { }

    public override Task<Video?> GetByIdAsync<TKey>(TKey id, CancellationToken cancellationToken = default) {
        return GetQueryable()
            .Include(x => x.User)
            .FirstOrDefaultAsync(x => x.Id.Equals(id));
    }

    public Task<List<Video>> GetPageWithUserAsync<TKey>(int pageNumber,
        int pageSize,
        Func<Video, TKey>? keySelector = null,
        bool desc = false,
        CancellationToken cancellationToken = default)
    {
        var queryable = GetQueryable()
            .Include(x => x.User);

        List<Video> result = keySelector switch {
            not null => desc
                ? queryable
                    .OrderByDescending(keySelector)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToList()
                : queryable
                    .OrderBy(keySelector)
                    .OrderByDescending(keySelector)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToList(),
            _ => queryable
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList()
        };

        return Task.FromResult(result);
    }

    public Task<List<Comment>> GetCommentsAsync(string id, CancellationToken cancellationToken = default) {
        return GetQueryable()
            .Where(x => x.Id.Equals(id))
            .SelectMany(x => x.Comments)
            .Include(x => x.User)
            .ToListAsync(cancellationToken);
    }

    public Task<Comment?> GetCommentByIdAsync(string videoId, string commentId, CancellationToken cancellationToken = default) {
        return GetQueryable()
            .Where(x => x.Id.Equals(videoId))
            .SelectMany(x => x.Comments)
            .FirstOrDefaultAsync(x => x.Id.Equals(commentId), cancellationToken: cancellationToken);
    }

    public Task<List<Comment>> GetCommentsPageAsync(string id, int pageNumber, int pageSize, CancellationToken cancellationToken = default) {
        return GetQueryable()
            .Where(x => x.Id.Equals(id))
            .SelectMany(x => x.Comments)
            .Include(x => x.User)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }

    public Task<int> GetCommentsCountAsync(string id, CancellationToken cancellationToken = default) {
        return GetQueryable()
            .Where(x => x.Id.Equals(id))
            .SelectMany(x => x.Comments)
            .CountAsync(cancellationToken);
    }

    public Task<Reaction?> GetReactionAsync(string id, string userId, CancellationToken cancellationToken = default) {
        return GetQueryable()
            .Where(x => x.Id.Equals(id))
            .SelectMany(x => x.Reactions)
            .Where(x => x.UserId.Equals(userId))
            .FirstOrDefaultAsync(cancellationToken);
    }

    public Task<Reaction?> GetReactionOnCommentAsync(string videoId, string commentId, string userId, CancellationToken cancellationToken = default) {
        return GetQueryable()
            .Where(x => x.Id.Equals(videoId))
            .SelectMany(x => x.Comments)
            .Where(x => x.Id.Equals(commentId))
            .SelectMany(x => x.Reactions)
            .Where(x => x.UserId.Equals(userId))
            .FirstOrDefaultAsync(cancellationToken);
    }

    public Task<int> GetReactionsCountAsync(string id, Expression<Func<Reaction, bool>>? predicate, CancellationToken cancellationToken = default) {
        return GetQueryable()
            .Where(x => x.Id.Equals(id))
            .SelectMany(x => x.Reactions)
            .Where(predicate ?? (x => true))
            .CountAsync(cancellationToken);
    }

    public Task<int> GetReactionsOnCommentCountAsync(string videoId,
        string commentId,
        Expression<Func<Reaction, bool>>? predicate,
        CancellationToken cancellationToken = default)
    {
        return GetQueryable()
            .Where(x => x.Id.Equals(videoId))
            .SelectMany(x => x.Comments)
            .Where(x => x.Id.Equals(commentId))
            .SelectMany(x => x.Reactions)
            .Where(predicate ?? (x => true))
            .CountAsync(cancellationToken);
    }

    public Task<int> DeleteReactionAsync(string id, string userId, CancellationToken cancellationToken = default) {
        return GetQueryable()
            .Where(x => x.Id.Equals(id))
            .SelectMany(x => x.Reactions)
            .Where(x => x.UserId.Equals(userId))
            .ExecuteDeleteAsync(cancellationToken);
    }
}