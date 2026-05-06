using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Shared.Common.Extensions;
using Shared.Common.ResultPattern;

namespace CrossReview.Application.Project.UseCases.UpdateProjectDescription;

public class UpdateProjectDescriptionUseCase
{
    private readonly ILogger<UpdateProjectDescriptionUseCase> _logger;
    private readonly IValidator<UpdateProjectDescriptionRequest> _validator;
    private readonly IProjectRepository _repository;

    public UpdateProjectDescriptionUseCase(
        ILogger<UpdateProjectDescriptionUseCase> logger,
        IValidator<UpdateProjectDescriptionRequest> validator,
        IProjectRepository repository)
    {
        _logger = logger;
        _validator = validator;
        _repository = repository;
    }

    public async Task<Result<Guid, Errors>> Execute(UpdateProjectDescriptionRequest request,
        CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        
        if (!validationResult.IsValid)
            return validationResult.ToList();

        var project = await _repository.GetByIdAsync(request.ProjectId, cancellationToken);
        
        if (project is null)
            return GeneralErrors.NotFound(request.ProjectId).ToErrors();
        
        project.UpdateDescription(request.Description);
        
        _repository.SaveAsync(project, cancellationToken);
        
        _logger.LogInformation("Description of project {ProjectId} was updated", project.Id);

        return project.Id;
    }
}