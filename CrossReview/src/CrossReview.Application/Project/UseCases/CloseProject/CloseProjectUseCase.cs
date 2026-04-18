using CrossReview.Domain.Project;
using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Shared.Common.Extensions;
using Shared.Common.ResultPattern;

namespace CrossReview.Application.Project.UseCases.CloseProject;

public class CloseProjectUseCase
{
    private readonly ILogger<CloseProjectRequest> _logger;
    private readonly IValidator<CloseProjectRequest> _validator;
    private readonly IProjectRepository _repository;
    
    public CloseProjectUseCase(
        ILogger<CloseProjectRequest> logger, 
        IValidator<CloseProjectRequest> validator,
        IProjectRepository repository)
    {
        _logger = logger;
        _validator = validator;
        _repository = repository;
    }

    public async Task<Result<Guid, Errors>> Execute(CloseProjectRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return validationResult.ToList();

        var project = await _repository.GetByIdAsync(request.ProjectId, cancellationToken);

        if (project is null)
            return GeneralErrors.NotFound(request.ProjectId).ToErrors();
        
        project.ToDeactivate();

        await _repository.SaveAsync(project, cancellationToken);
        
        _logger.LogInformation("Project {ProjectId} has been closed", project.Id);
        
        return project.Id;
    }
}