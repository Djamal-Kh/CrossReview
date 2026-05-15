using CrossReview.Application.Project.DTOs;
using CrossReview.Domain.Project;
using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using Shared.Common.ResultPattern;

namespace CrossReview.Application.Project.UseCases.GetProjectMemberById;

public class GetProjectMemberByIdUseCase
{
    private readonly ILogger<GetProjectMemberByIdUseCase> _logger;
    private readonly IProjectRepository _repository;
    
    public GetProjectMemberByIdUseCase(
        ILogger<GetProjectMemberByIdUseCase> logger, 
        IProjectRepository repository)
    {
        _logger = logger;
        _repository = repository;
    }

    public async Task<Result<ProjectMemberDto, Errors>> Execute(GetProjectMemberByIdRequest request,
        CancellationToken cancellationToken)
    {
        var project = await _repository.GetByIdAsync(request.ProjectId, cancellationToken);
        
        if (project is null)
            return GeneralErrors.NotFound(request.ProjectId).ToErrors();

        var member = project.Members.FirstOrDefault(m => m.UserId == request.UserId);
        
        if (member is null)
            return GeneralErrors.NotFound(request.ProjectId).ToErrors();

        _logger.LogInformation("ProjectMember {UserId} was founded", member.UserId);

        var result = new ProjectMemberDto
        {
            UserId = member.UserId,
            Role = member.Role,
            IsActive = member.IsActive,
            JoinedAt = member.JoinedAt,
            LeftAt = member.LeftAt
        };
        
        return result;
    }
}