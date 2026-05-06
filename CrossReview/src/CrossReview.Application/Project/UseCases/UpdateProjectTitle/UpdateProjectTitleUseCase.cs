using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Shared.Common.Extensions;
using Shared.Common.ResultPattern;

namespace CrossReview.Application.Project.UseCases.UpdateProjectTitle;

public class UpdateProjectTitleUseCase
{
    private readonly ILogger<UpdateProjectTitleUseCase> _logger;
    private readonly IValidator<UpdateProjectTitleRequest> _validator;
    private readonly IProjectRepository _repository;

    public UpdateProjectTitleUseCase(
        ILogger<UpdateProjectTitleUseCase> logger,
        IValidator<UpdateProjectTitleRequest> validator, 
        IProjectRepository repository)
    {
        _logger = logger;
        _validator = validator;
        _repository = repository;
    }

    public async Task<Result<Guid, Errors>> Execute(UpdateProjectTitleRequest request,
        CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        
        if (!validationResult.IsValid)
            return validationResult.ToList();

        var project = await _repository.GetByIdAsync(request.ProjectId, cancellationToken);
        
        if (project is null)
            return GeneralErrors.NotFound(request.ProjectId).ToErrors();
        
        project.UpdateTitle(request.Title);
        
        await _repository.SaveAsync(project, cancellationToken);
        
        _logger.LogInformation("Title of project {ProjectId} was updated", project.Id);

        return project.Id;
    }
}