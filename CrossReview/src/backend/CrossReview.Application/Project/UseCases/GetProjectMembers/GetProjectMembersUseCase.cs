using CrossReview.Domain.Project;
using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using Shared.Common.ResultPattern;

namespace CrossReview.Application.Project.UseCases.GetProjectMembers;

public class GetProjectMembersUseCase
{
    private readonly ILogger<GetProjectMembersUseCase> _logger;
    private readonly IProjectRepository _repository;
    
    public GetProjectMembersUseCase(
        ILogger<GetProjectMembersUseCase> logger, 
        IProjectRepository repository)
    {
        _logger = logger;
        _repository = repository;
    }

    public async Task<Result<IReadOnlyCollection<ProjectMember>, Errors>> Execute(GetProjectMembersRequest request,
        CancellationToken cancellationToken)
    {
        var project = await _repository.GetByIdAsync(request.ProjectId, cancellationToken);
        
        if (project is null)
            return GeneralErrors.NotFound(request.ProjectId).ToErrors();

        var members = project.Members;
        
        if (members.Count == 0)
            return GeneralErrors.CollectionEmpty().ToErrors();

        _logger.LogInformation("ProjectMembers of Project {ProjectId} was returned", request.ProjectId);
        
        // Компилятор не дает напрямую вернуть members
        return Result.Success<IReadOnlyCollection<ProjectMember>, Errors>(members);
    }
}