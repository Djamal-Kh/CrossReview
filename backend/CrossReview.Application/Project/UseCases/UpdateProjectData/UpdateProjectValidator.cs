using FluentValidation;
using Shared.Common.Extensions;
using Shared.Common.ResultPattern;

namespace CrossReview.Application.Project.UseCases.UpdateProjectData;

public class UpdateProjectValidator : AbstractValidator<UpdateProjectRequest>
{
    public UpdateProjectValidator()
    {
        RuleFor(x => x.ProjectId)
            .NotEmpty()
            .WithError(GeneralErrors.ValueIsRequired(nameof(UpdateProjectRequest.ProjectId)));
        
        RuleFor(x => x.Title)
            .NotEmpty()
            .WithError(GeneralErrors.ValueIsRequired(nameof(UpdateProjectRequest.Title)))
            .MaximumLength(200)
            .WithError(GeneralErrors.ValueTooLong(200, nameof(UpdateProjectRequest.Title)))
            .MinimumLength(3)
            .WithError(GeneralErrors.ValueTooShort(3, nameof(UpdateProjectRequest.Title)));
        
        RuleFor(x => x.Description)
            .MaximumLength(1000)
            .WithError(GeneralErrors.ValueTooLong(1000, nameof(UpdateProjectRequest.Description)))
            .When(x => !string.IsNullOrWhiteSpace(x.Description));
    }
}