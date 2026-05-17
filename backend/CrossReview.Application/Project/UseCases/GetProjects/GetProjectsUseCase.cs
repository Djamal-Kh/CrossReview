using CrossReview.Application.Project.DTOs;
using CrossReview.Domain.Project;
using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using Shared.Common.ResultPattern;

namespace CrossReview.Application.Project.UseCases.GetProjects;

public class GetProjectsUseCase
{
    private readonly ILogger<GetProjectsUseCase> _logger;
    private readonly IProjectRepository _repository;
    
    public GetProjectsUseCase(
        ILogger<GetProjectsUseCase> logger, 
        IProjectRepository repository)
    {
        _logger = logger;
        _repository = repository;
    }

    public async Task<Result<List<ProjectListItemDto>, Errors>> Execute(CancellationToken cancellationToken)
    {
        var projects = await _repository.GetAllAsync(cancellationToken);

        if (projects.Count == 0)
            return GeneralErrors.CollectionEmpty().ToErrors();

        var result = projects.Select(p => new ProjectListItemDto
        {
            Id = p.Id,
            Title =  p.Title,
            Status =  p.Status,
            MembersCount = p.Members.Count,
            ActiveReviewPeriods = p.ReviewPeriods
                .Count(rp => rp.Status == EnumReviewPeriodStatus.Active)
        }).ToList();
        
        return result;
    }
}