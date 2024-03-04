using MediatR;
using VideoSharingPlatform.Core.Common;

namespace VideoSharingPlatform.Application.Features.Commands;

public interface ICommand<TResponse> : IRequest<Result<TResponse, ExceptionBase>>;