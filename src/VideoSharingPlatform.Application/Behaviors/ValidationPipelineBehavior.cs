using FluentValidation;
using MediatR;
using VideoSharingPlatform.Application.Extensions;
using VideoSharingPlatform.Core.Common;

namespace VideoSharingPlatform.Application.Behaviors;

public class ValidationPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly IValidator<TRequest> _validator;
    public ValidationPipelineBehavior(IValidator<TRequest> validator) => _validator = validator;

    public async Task<TResponse> Handle(TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var result = await _validator.ValidateAsync(request, cancellationToken);

        if (!result.IsValid) {
            var successType = typeof(TResponse).GetGenericArguments()[0];
            var errorType = typeof(IEnumerable<Error>);
            var resultType = typeof(Result<,>).MakeGenericType(successType, errorType);

            var resultObject = Activator.CreateInstance(resultType, [result.Errors.Select(x => x.MapToError())]);

            if (resultObject is not null) return (TResponse) resultObject;

            throw new Exception("Failed to Create an Instance of Result<,>");
        }

        return await next();
    }
}