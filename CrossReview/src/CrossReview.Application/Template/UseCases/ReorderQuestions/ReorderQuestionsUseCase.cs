using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using Shared.Common.ResultPattern;

namespace CrossReview.Application.Template.UseCases.ReorderQuestions;

public class ReorderQuestionsUseCase
{
    private readonly ILogger<ReorderQuestionsUseCase> _logger;
    private readonly ITemplateRepository _templateRepository;
    
    public ReorderQuestionsUseCase(
        ILogger<ReorderQuestionsUseCase> logger, 
        ITemplateRepository templateRepository)
    {
        _logger = logger;
        _templateRepository = templateRepository;
    }

    // Пока что больше как заглушка, потом подумай как нормально реализовать
    public async Task<Result<Guid, Errors>> Execute(ReorderQuestionsRequest request, CancellationToken cancellationToken)
    {
        var template = await _templateRepository.GetByIdAsync(request.TemplateId);

        if (template is null)
            return GeneralErrors.NotFound(request.TemplateId).ToErrors();

        var questions = template.Questions.ToList();

        var orderedList = questions.OrderByDescending(r => r.Weight);

        await _templateRepository.SaveAsync(template, cancellationToken);

        return template.Id;
    }
}