using FluentValidation;
using Shared.Common.Extensions;
using Shared.Common.ResultPattern;

namespace CrossReview.Application.Project.UseCases.CreateProject;

public class CreateProjectValidator : AbstractValidator<CreateProjectRequest>
{
    public CreateProjectValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .WithError(GeneralErrors.ValueIsRequired(nameof(CreateProjectRequest.Title)))
            .MaximumLength(200)
            .WithError(GeneralErrors.ValueTooLong(200, nameof(CreateProjectRequest.Title)))
            .MinimumLength(3)
            .WithError(GeneralErrors.ValueTooShort(3, nameof(CreateProjectRequest.Title)));
        
        RuleFor(x => x.Description)
            .MaximumLength(1000)
            .WithError(GeneralErrors.ValueTooLong(1000, nameof(CreateProjectRequest.Description)))
            .When(x => !string.IsNullOrWhiteSpace(x.Description));
    }
}