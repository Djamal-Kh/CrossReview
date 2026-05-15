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

    public async Task<Result<ProjectEntity, Errors>> Execute(GetProjectByIdRequest request,
        CancellationToken cancellationToken)
    {
        var project = await _repository.GetByIdAsync(request.ProjectId, cancellationToken);

        if (project is null)
            return GeneralErrors.NotFound(request.ProjectId).ToErrors();

        _logger.LogInformation("Project {ProjectId} was returned", project.Id);
        
        return project;
    }
}