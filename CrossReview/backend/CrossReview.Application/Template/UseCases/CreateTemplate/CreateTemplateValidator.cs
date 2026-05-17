using FluentValidation;
using Shared.Common.Extensions;
using Shared.Common.ResultPattern;

namespace CrossReview.Application.Template.UseCases.CreateTemplate;

public class CreateTemplateValidator : AbstractValidator<CreateTemplateRequest>
{
    public CreateTemplateValidator()
    {
        RuleFor(x => x.ProjectId)
            .NotEmpty()
            .WithError(GeneralErrors.ValueIsRequired(nameof(CreateTemplateRequest.ProjectId)));
        
        RuleFor(x => x.Title)
            .NotEmpty()
            .WithError(GeneralErrors.ValueIsRequired(nameof(CreateTemplateRequest.Title)))
            .MaximumLength(200)
            .WithError(GeneralErrors.ValueTooLong(200, nameof(CreateTemplateRequest.Title)))
            .MinimumLength(3)
            .WithError(GeneralErrors.ValueTooShort(3, nameof(CreateTemplateRequest.Title)));
    }
}