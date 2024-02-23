using FluentValidation;
using MediatR;
using VideoSharingPlatform.Application.Extensions;
using VideoSharingPlatform.Core.Common;

namespace VideoSharingPlatform.Application.Behaviors;

public class ValidationPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;
    public ValidationPipelineBehavior(IEnumerable<IValidator<TRequest>> validators) => _validators = validators;

    public async Task<TResponse> Handle(TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (!_validators.Any()) {
            return await next();
        }

        var result = _validators
            .Select(validator => validator.ValidateAsync(request, cancellationToken).GetAwaiter().GetResult())
            .Aggregate((total, next) => {
                total.Errors.AddRange(next.Errors);
                return total;
            });

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