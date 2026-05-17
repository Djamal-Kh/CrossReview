using CrossReview.Application.Project.DTOs;
using CrossReview.Application.Template.DTOs;
using CrossReview.Domain.Project;
using CSharpFunctionalExtensions;
using Shared.Common.ResultPattern;

namespace CrossReview.Application.Template.UseCases.GetAllTemplates;

public class GetAllTemplateUseCase
{
    private readonly ITemplateRepository _templateRepository;
    
    public GetAllTemplateUseCase(ITemplateRepository templateRepository)
    {
        _templateRepository = templateRepository;
    }
    
    public async Task<Result<List<TemplateDto>, Errors>> Execute(CancellationToken cancellationToken)
    {
        var templates = await _templateRepository.GetAllAsync(cancellationToken);

        var result = templates.Select(t => new TemplateDto
        {
            Id = t.Id,
            ProjectId = t.ProjectId,
            Title = t.Title,
            IsActive = t.IsActive,
            Questions = t.Questions.Select(q => new ReviewQuestionDto
            {
                Id = q.Id,
                Title = q.Title,
                Weight = q.Weight,
            }).ToList()
        }).ToList();

        return result;
    }
}