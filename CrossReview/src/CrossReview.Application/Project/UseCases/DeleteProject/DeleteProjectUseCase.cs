using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Shared.Common.Extensions;
using Shared.Common.ResultPattern;

namespace CrossReview.Application.Project.UseCases.DeleteProject;

public class DeleteProjectUseCase
{
    private readonly ILogger<DeleteProjectUseCase> _logger;
    private readonly IValidator<DeleteProjectRequest> _validator;
    private readonly IProjectRepository _repository;
    
    public DeleteProjectUseCase(ILogger<DeleteProjectUseCase> logger, IValidator<DeleteProjectRequest> validator, IProjectRepository repository)
    {
        _logger = logger;
        _validator = validator;
        _repository = repository;
    }

    public async Task<Result<Guid, Errors>> Execute(DeleteProjectRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return validationResult.ToList();

        var project = await _repository.GetByIdAsync(request.ProjectId, cancellationToken);

        if (project is null)
            return GeneralErrors.NotFound(request.ProjectId).ToErrors();

        await _repository.DeleteAsync(project, cancellationToken);
        
        _logger.LogInformation("Project {ProjectId} has been deleted", project.Id);
        
        return project.Id;
    }
}