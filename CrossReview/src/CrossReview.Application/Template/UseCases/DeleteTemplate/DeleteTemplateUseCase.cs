using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using Shared.Common.ResultPattern;

namespace CrossReview.Application.Template.UseCases.DeleteTemplate;

public class DeleteTemplateUseCase
{
    private readonly ILogger<DeleteTemplateUseCase> _logger;
    private readonly ITemplateRepository _templateRepository;
    
    public DeleteTemplateUseCase(
        ILogger<DeleteTemplateUseCase> logger,
        ITemplateRepository templateRepository)
    {
        _logger = logger;
        _templateRepository = templateRepository;
    }

    public async Task<Result<Guid, Errors>> Execute(DeleteTemplateRequest request, CancellationToken cancellationToken)
    {
        var template = await _templateRepository.GetByIdAsync(request.TemplateId);

        if (template is null)
            return GeneralErrors.NotFound(request.TemplateId).ToErrors();
        
        await _templateRepository.DeleteAsync(template, cancellationToken);
        
        _logger.LogInformation("Че-нибудь напишешь");
        
        return template.Id;
    }
    
}