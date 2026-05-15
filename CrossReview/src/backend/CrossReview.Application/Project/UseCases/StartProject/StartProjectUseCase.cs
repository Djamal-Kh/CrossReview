using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Shared.Common.Extensions;
using Shared.Common.ResultPattern;

namespace CrossReview.Application.Project.UseCases.StartProject;

public class StartProjectUseCase
{
    private readonly ILogger<StartProjectUseCase> _logger;
    private readonly IValidator<StartProjectRequest> _validator;
    private readonly IProjectRepository _repository;
    
    public StartProjectUseCase(
        IValidator<StartProjectRequest> validator, 
        IProjectRepository repository,
        ILogger<StartProjectUseCase> logger)
    {
        _validator = validator;
        _repository = repository;
        _logger = logger;
    }

    public async Task<Result<Guid, Errors>> Execute(StartProjectRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        
        if (!validationResult.IsValid)
            return validationResult.ToList();

        var project = await _repository.GetByIdAsync(request.ProjectId, cancellationToken);
        
        if (project is null)
            return GeneralErrors.NotFound(request.ProjectId).ToErrors();
        
        var result = project.ToActivate();

        if (result.IsFailure)
            return result.Error;
        
        await _repository.SaveAsync(cancellationToken);
        
        _logger.LogInformation("Project {ProjectId} was activated", project.Id);

        return project.Id;
    }
}