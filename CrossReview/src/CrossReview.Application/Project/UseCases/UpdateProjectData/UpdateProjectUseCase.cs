using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Shared.Common.Extensions;
using Shared.Common.ResultPattern;

namespace CrossReview.Application.Project.UseCases.UpdateProjectData;

public class UpdateProjectUseCase
{
    private readonly ILogger<UpdateProjectUseCase> _logger;
    private readonly IValidator<UpdateProjectRequest> _validator;
    private readonly IProjectRepository _repository;
    
    public UpdateProjectUseCase(
        ILogger<UpdateProjectUseCase> logger, 
        IValidator<UpdateProjectRequest> validator,
        IProjectRepository repository)
    {
        _logger = logger;
        _validator = validator;
        _repository = repository;
    }

    public async Task<Result<Guid, Errors>> Execute(UpdateProjectRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        
        if (!validationResult.IsValid)
            return validationResult.ToList();

        var project = await _repository.GetByIdAsync(request.ProjectId, cancellationToken);
        
        if (project is null)
            return GeneralErrors.NotFound(request.ProjectId).ToErrors();
        
        project.UpdateData(request.Title, request.Description);
        
        _logger.LogInformation("Data of project {ProjectId} was updated", project.Id);

        return project.Id;
    }
}