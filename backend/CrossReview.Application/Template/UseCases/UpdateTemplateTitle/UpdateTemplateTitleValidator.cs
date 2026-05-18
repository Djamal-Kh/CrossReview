using FluentValidation;
using Shared.Common.Extensions;
using Shared.Common.ResultPattern;

namespace CrossReview.Application.Template.UseCases.UpdateTemplateTitle;

public class UpdateTemplateTitleValidator : AbstractValidator<UpdateTemplateTitleRequest>
{
    public UpdateTemplateTitleValidator()
    {
        RuleFor(x => x.TemplateId)
            .NotEmpty()
            .WithError(GeneralErrors.ValueIsRequired(nameof(UpdateTemplateTitleRequest.TemplateId)));
        
        RuleFor(x => x.Title)
            .NotEmpty()
            .WithError(GeneralErrors.ValueIsRequired(nameof(UpdateTemplateTitleRequest.Title)))
            .MaximumLength(200)
            .WithError(GeneralErrors.ValueTooLong(200, nameof(UpdateTemplateTitleRequest.Title)))
            .MinimumLength(3)
            .WithError(GeneralErrors.ValueTooShort(3, nameof(UpdateTemplateTitleRequest.Title)));
    }
}