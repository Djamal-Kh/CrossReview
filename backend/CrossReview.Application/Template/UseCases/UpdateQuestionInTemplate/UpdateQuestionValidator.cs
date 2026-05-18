using FluentValidation;
using Shared.Common.Extensions;
using Shared.Common.ResultPattern;

namespace CrossReview.Application.Template.UseCases.UpdateQuestionInTemplate;

public class UpdateQuestionValidator : AbstractValidator<UpdateQuestionRequest>
{
    public UpdateQuestionValidator()
    {
        RuleFor(x => x.TemplateId)
            .NotEmpty()
            .WithError(GeneralErrors.ValueIsRequired(nameof(UpdateQuestionRequest.TemplateId)));
        
        RuleFor(x => x.QuestionId)
            .NotEmpty()
            .WithError(GeneralErrors.ValueIsRequired(nameof(UpdateQuestionRequest.QuestionId)));
        
        RuleFor(x => x.Title)
            .MaximumLength(500)
            .WithError(GeneralErrors.ValueTooLong(500, nameof(UpdateQuestionRequest.Title)))
            .MinimumLength(3)
            .When(x => x.Title is not null)
            .WithError(GeneralErrors.ValueTooShort(3, nameof(UpdateQuestionRequest.Title)));
        
        RuleFor(x => x.Weight)
            .InclusiveBetween(0, 10)
            .When(x => x.Weight.HasValue)
            .WithError(GeneralErrors.ValueIsInvalid(nameof(UpdateQuestionRequest.Weight)));
        
        RuleFor(x => x)
            .Must(x => x.Title is not null || x.Weight is not null)
            .WithError(GeneralErrors.ValueIsInvalid("At least one field (Title or Weight) must be provided for update"));
    }
}