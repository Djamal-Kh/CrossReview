using FluentValidation;
using Shared.Common.Extensions;
using Shared.Common.ResultPattern;

namespace CrossReview.Application.Project.UseCases.UpdateProjectTitle;

public class UpdateProjectTitleValidator : AbstractValidator<UpdateProjectTitleRequest>
{
    public UpdateProjectTitleValidator()
    {
        RuleFor(x => x.ProjectId)
            .NotEmpty()
            .WithError(GeneralErrors.ValueIsRequired(nameof(UpdateProjectTitleRequest.ProjectId)));
        
        RuleFor(x => x.Title)
            .NotEmpty()
            .WithError(GeneralErrors.ValueIsRequired(nameof(UpdateProjectTitleRequest.Title)))
            .MaximumLength(200)
            .WithError(GeneralErrors.ValueTooLong(200, nameof(UpdateProjectTitleRequest.Title)))
            .MinimumLength(3)
            .WithError(GeneralErrors.ValueTooShort(3, nameof(UpdateProjectTitleRequest.Title)));
    }
}