namespace VideoSharingPlatform.Application.Features.Queries.GetCommentsCount;

public record GetCommentsCountQuery(string VideoId) : IQuery<int>;