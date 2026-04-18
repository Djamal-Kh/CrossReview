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

    public async Task<Result<List<ProjectEntity>, Errors>> Execute(CancellationToken cancellationToken)
    {
        // Мб сделать для определенного пользователя ? Тип все проекты в которых состоит пользователь с соответствующим UserId
        var projects = await _repository.GetAllAsync();

        if (projects.Count == 0)
            return GeneralErrors.CollectionEmpty().ToErrors();

        return projects;
    }
}