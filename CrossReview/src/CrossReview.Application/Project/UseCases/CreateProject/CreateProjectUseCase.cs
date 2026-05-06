using CrossReview.Domain.Project;
using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Shared.Common.Extensions;
using Shared.Common.ResultPattern;

namespace CrossReview.Application.Project.UseCases.CreateProject;


public class CreateProjectUseCase
{
    private readonly ILogger<CreateProjectUseCase> _logger;
    private readonly IValidator<CreateProjectRequest> _validator;
    private readonly IProjectRepository _repository;
    
    public CreateProjectUseCase(
        ILogger<CreateProjectUseCase> logger, 
        IValidator<CreateProjectRequest> validator, IProjectRepository repository)
    {
        _logger = logger;
        _validator = validator;
        _repository = repository;
    }

    public async Task<Result<Guid, Errors>> Execute(CreateProjectRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return validationResult.ToList();
        
        var isTitleExist = await _repository.ExistsByTitleAsync(request.Title, cancellationToken);
        
        if (isTitleExist)
            return GeneralErrors.ValueAlreadyExists(request.Title).ToErrors();
        
        var project = ProjectEntity.Create(request.Title, request.Description);
        
        var result = await _repository.AddAsync(project, cancellationToken);
        
        if (result.IsFailure)
        {
            _logger.LogWarning("Failed to create project: {Title}", request.Title);
            return GeneralErrors.Failure().ToErrors();
        }
        
        _logger.LogInformation("Project with title {Title} was created", request.Title);

        return result.Value;
    }
}