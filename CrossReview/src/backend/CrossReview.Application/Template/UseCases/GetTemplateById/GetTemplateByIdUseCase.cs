using CrossReview.Application.Template.DTOs;
using CrossReview.Domain.Template;
using CSharpFunctionalExtensions;
using Shared.Common.ResultPattern;

namespace CrossReview.Application.Template.UseCases.GetTemplateById;

public class GetTemplateByIdUseCase
{
    private readonly ITemplateRepository _templateRepository;
    
    public GetTemplateByIdUseCase(ITemplateRepository templateRepository)
    {
        _templateRepository = templateRepository;
    }

    public async Task<Result<TemplateDto, Errors>> Execute(GetTemplateByIdRequest request,
        CancellationToken cancellationToken)
    {
        var template = await _templateRepository.GetByIdAsync(request.TemplateId);

        if (template is null)
            return GeneralErrors.NotFound(request.TemplateId).ToErrors();

        var questionDtos = template.Questions
            .Select(q => new ReviewQuestionDto
            {
                Id = q.Id,
                Title = q.Title,
                Weight = q.Weight,
            }).ToList();

        var result = new TemplateDto
        {
            Id = template.Id,
            ProjectId = template.ProjectId,
            Title = template.Title,
            IsActive = template.IsActive,
            Questions = questionDtos
        };
        
        return result;
    }
}