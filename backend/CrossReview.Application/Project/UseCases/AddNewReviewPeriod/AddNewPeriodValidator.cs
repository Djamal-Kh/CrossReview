using FluentValidation;
using Shared.Common.Extensions;
using Shared.Common.ResultPattern;

namespace CrossReview.Application.Project.UseCases.AddNewReviewPeriod;

public class AddNewPeriodValidator : AbstractValidator<AddNewPeriodRequest>
{
    public AddNewPeriodValidator()
    {
        RuleFor(s => s.StartDate)
            .NotEmpty()
            .WithError(GeneralErrors.ValueIsRequired(nameof(AddNewPeriodRequest.StartDate)));
            
        RuleFor(s => s.StartDate)
            .GreaterThanOrEqualTo(DateTime.UtcNow)
            .WithError(GeneralErrors.ValueIsInvalid());

        RuleFor(reviewPeriod => reviewPeriod.EndDate)
            .NotEmpty()
            .WithError(GeneralErrors.ValueIsRequired(nameof(AddNewPeriodRequest.EndDate)));

        RuleFor(x => x)
            .Must(x => x.StartDate < x.EndDate)
            .WithError(GeneralErrors.ValueIsInvalid());
        
        RuleFor(x => x)
            .Must(x => (x.EndDate - x.StartDate).TotalDays <= 30)
            .WithError(GeneralErrors.ValueIsInvalid());
    }
}