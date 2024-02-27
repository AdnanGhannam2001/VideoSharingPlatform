namespace VideoSharingPlatform.Application.Features.Queries.GetCommentsCount;

public record GetCommentsCountQuery(string Id) : IQuery<int>;