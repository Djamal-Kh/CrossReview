using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using Shared.Common.ResultPattern;

namespace CrossReview.Application.Template.UseCases.DeactivateTemplate;

public class DeactivateTemplateUseCase
{
    private readonly ILogger<DeactivateTemplateUseCase> _logger;
    private readonly ITemplateRepository _templateRepository;
    
    public DeactivateTemplateUseCase(
        ILogger<DeactivateTemplateUseCase> logger, 
        ITemplateRepository templateRepository)
    {
        _logger = logger;
        _templateRepository = templateRepository;
    }

    public async Task<Result<Guid, Errors>> Execute(DeactivateTemplateRequest request,
        CancellationToken cancellationToken)
    {
        var template = await _templateRepository.GetByIdAsync(request.TemplateId);

        if (template is null)
            return GeneralErrors.NotFound(request.TemplateId).ToErrors();
        
        template.Deactivate();

        await _templateRepository.SaveAsync(cancellationToken);
        
        return template.Id;
    }
}