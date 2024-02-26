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

    public override Task<List<Video>> GetPageAsync<TKey>(int pageNumber,
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
}