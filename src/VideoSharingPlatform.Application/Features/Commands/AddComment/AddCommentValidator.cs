using FluentValidation;

namespace VideoSharingPlatform.Application.Features.Commands.AddComment;

public class AddCommentValidator : AbstractValidator<AddCommentCommand> {
    public AddCommentValidator() {
        RuleFor(x => x.Content)
            .MaximumLength(1000);
    }
}