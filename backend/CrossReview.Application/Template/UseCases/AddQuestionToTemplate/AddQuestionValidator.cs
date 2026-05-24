using FluentValidation;
using Shared.Common.Extensions;
using Shared.Common.ResultPattern;

namespace CrossReview.Application.Template.UseCases.AddQuestionToTemplate;

public class AddQuestionValidator : AbstractValidator<AddQuestionRequest>
{
    public AddQuestionValidator()
    {
        RuleFor(x => x.TemplateId)
            .NotEmpty()
            .WithError(GeneralErrors.ValueIsRequired(nameof(AddQuestionRequest.TemplateId)));
        
        RuleFor(x => x.Title)
            .NotEmpty()
            .WithError(GeneralErrors.ValueIsRequired(nameof(AddQuestionRequest.Title)))
            .MaximumLength(500)
            .WithError(GeneralErrors.ValueTooLong(500, nameof(AddQuestionRequest.Title)))
            .MinimumLength(3)
            .WithError(GeneralErrors.ValueTooShort(3, nameof(AddQuestionRequest.Title)));
        
        RuleFor(x => x.Weight)
            .InclusiveBetween(0.1, 10)
            .WithError(GeneralErrors.ValueIsInvalid(nameof(AddQuestionRequest.Weight)))
            .Must(weight => weight >= 0)
            .WithError(GeneralErrors.ValueIsInvalid(nameof(AddQuestionRequest.Weight)));
    }
}