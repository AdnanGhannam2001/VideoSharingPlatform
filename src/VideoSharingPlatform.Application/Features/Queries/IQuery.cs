using MediatR;
using VideoSharingPlatform.Core.Common;

namespace VideoSharingPlatform.Application.Features.Queries;

public interface IQuery<TResponse> : IRequest<Result<TResponse, IEnumerable<Error>>>;