using FluentValidation;

namespace VideoSharingPlatform.Application.Features.Commands.CreateVideo;

public class CreateVideoValidator : AbstractValidator<CreateVideoCommand> {
    public CreateVideoValidator() {
        RuleFor(x => x.Title)
            .MaximumLength(75)
            .NotEmpty();

        RuleFor(x => x.Description)
            .MaximumLength(1000)
            .NotEmpty();
    }
}