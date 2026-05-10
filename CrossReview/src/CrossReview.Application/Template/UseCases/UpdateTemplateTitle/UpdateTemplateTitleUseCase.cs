using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Shared.Common.ResultPattern;

namespace CrossReview.Application.Template.UseCases.UpdateTemplateTitle;

public class UpdateTemplateTitleUseCase
{
    private readonly ILogger<UpdateTemplateTitleUseCase> _logger;
    private readonly IValidator<UpdateTemplateTitleRequest> _validator;
    private readonly ITemplateRepository _templateRepository;
    
    public UpdateTemplateTitleUseCase(
        ILogger<UpdateTemplateTitleUseCase> logger,
        IValidator<UpdateTemplateTitleRequest> validator, 
        ITemplateRepository templateRepository)
    {
        _logger = logger;
        _validator = validator;
        _templateRepository = templateRepository;
    }

    public async Task<Result<Guid, Errors>> Execute(UpdateTemplateTitleRequest request,
        CancellationToken cancellationToken)
    {
        var validatonResult = await _validator.ValidateAsync(request, cancellationToken);

        if (!validatonResult.IsValid)
            return GeneralErrors.ValueIsInvalid().ToErrors();

        var template = await _templateRepository.GetByIdAsync(request.TemplateId);

        if (template is null)
            return GeneralErrors.NotFound(request.TemplateId).ToErrors();
        
        template.UpdateTitle(request.Title);

        await _templateRepository.SaveAsync(cancellationToken);
        
        _logger.LogInformation("инфа");

        return template.Id;
    }
}