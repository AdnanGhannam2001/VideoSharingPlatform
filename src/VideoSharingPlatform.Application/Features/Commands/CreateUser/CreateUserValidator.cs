using FluentValidation;

namespace VideoSharingPlatform.Application.Features.Commands.CreateUser;

public class CreateUserValidator : AbstractValidator<CreateUserCommand> {
    public CreateUserValidator() {
        RuleFor(x => x.UserName)
            .Length(4, 25);

        RuleFor(x => x.Email)
            .EmailAddress();
    }
}