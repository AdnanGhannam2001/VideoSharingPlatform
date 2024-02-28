using Microsoft.EntityFrameworkCore;
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
}