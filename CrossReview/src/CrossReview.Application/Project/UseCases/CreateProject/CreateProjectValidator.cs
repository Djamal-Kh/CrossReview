using FluentValidation;
using Shared.Common.Extensions;
using Shared.Common.ResultPattern;

namespace CrossReview.Application.Project.UseCases.CreateProject;

public class CreateProjectValidator : AbstractValidator<CreateProjectRequest>
{
    public CreateProjectValidator()
    {
        RuleFor(request => request.Title)
            .NotEmpty().WithError(GeneralErrors.ValueIsRequired(nameof(CreateProjectRequest.Title)))
            .NotNull().WithError(GeneralErrors.ValueIsRequired(nameof(CreateProjectRequest.Title)));

        RuleFor(request => request.Description.Length)
            .LessThanOrEqualTo(1000).WithError(GeneralErrors.ValueTooLong(1000));

        RuleFor(request => request.Title.Length)
            .GreaterThanOrEqualTo(3).WithError(GeneralErrors.ValueTooShort(3))
            .LessThanOrEqualTo(100).WithError(GeneralErrors.ValueTooLong(100));
    }
}