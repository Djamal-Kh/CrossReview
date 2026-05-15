using CrossReview.Application.Project.DTOs;
using CrossReview.Domain.Project;
using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using Shared.Common.ResultPattern;

namespace CrossReview.Application.Project.UseCases.GetProjectById;

public class GetProjectByIdUseCase
{
    private readonly ILogger<GetProjectByIdUseCase> _logger;
    private readonly IProjectRepository _repository;
    
    public GetProjectByIdUseCase(
        ILogger<GetProjectByIdUseCase> logger, 
        IProjectRepository repository)
    {
        _logger = logger;
        _repository = repository;
    }

    public async Task<Result<ProjectDto, Errors>> Execute(GetProjectByIdRequest request,
        CancellationToken cancellationToken)
    {
        var project = await _repository.GetByIdAsync(request.ProjectId, cancellationToken);

        if (project is null)
            return GeneralErrors.NotFound(request.ProjectId).ToErrors();

        _logger.LogInformation("Project {ProjectId} was returned", project.Id);

        // маппинг
        var members = project.Members
            .Select(m => new ProjectMemberDto
            {
                UserId = m.UserId,
                Role = m.Role,
                IsActive = m.IsActive,
                JoinedAt = m.JoinedAt,
                LeftAt = m.LeftAt
            }).ToList();

        var reviewPeriods = project.ReviewPeriods
            .Select(rp => new ReviewPeriodDto
            {
                Id = rp.Id,
                StartDate = rp.StartDate,
                EndDate =  rp.EndDate,
                Status = rp.Status,
            }).ToList();
        
        var result = new ProjectDto()
        {
            Id = project.Id,
            Title = project.Title,
            Status = project.Status,
            Description = project.Description,
            Members = members,
            ReviewPeriods = reviewPeriods
        };
        
        return result;
    }
}