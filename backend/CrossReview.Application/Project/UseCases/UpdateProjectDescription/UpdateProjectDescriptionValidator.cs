using FluentValidation;
using Shared.Common.Extensions;
using Shared.Common.ResultPattern;

namespace CrossReview.Application.Project.UseCases.UpdateProjectDescription;

public class UpdateProjectDescriptionValidator : AbstractValidator<UpdateProjectDescriptionRequest>
{
    public UpdateProjectDescriptionValidator()
    {
        RuleFor(x => x.ProjectId)
            .NotEmpty()
            .WithError(GeneralErrors.ValueIsRequired(nameof(UpdateProjectDescriptionRequest.ProjectId)));
        
        RuleFor(x => x.Description)
            .MaximumLength(1000)
            .WithError(GeneralErrors.ValueTooLong(1000, nameof(UpdateProjectDescriptionRequest.Description)));
    }
}