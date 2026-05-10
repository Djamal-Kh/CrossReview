using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using Shared.Common.ResultPattern;

namespace CrossReview.Application.Template.UseCases.RemoveQuestionFromTemplate;

public class RemoveQuestionUseCase
{
    private readonly ILogger<RemoveQuestionUseCase> _logger;
    private readonly ITemplateRepository _templateRepository;
    
    public RemoveQuestionUseCase(
        ILogger<RemoveQuestionUseCase> logger, 
        ITemplateRepository templateRepository)
    {
        _logger = logger;
        _templateRepository = templateRepository;
    }

    public async Task<Result<Guid, Errors>> Execute(RemoveQuestionRequest request, CancellationToken cancellationToken)
    {
        var template = await _templateRepository.GetByIdAsync(request.TemplateId);

        if (template is null)
            return GeneralErrors.NotFound(request.TemplateId).ToErrors();
        
        template.RemoveQuestion(request.QuestionId);

        await _templateRepository.SaveAsync(cancellationToken);

        return template.Id;
    }
}