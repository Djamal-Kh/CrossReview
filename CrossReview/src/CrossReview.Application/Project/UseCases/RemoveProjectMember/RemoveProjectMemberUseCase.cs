using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Shared.Common.Extensions;
using Shared.Common.ResultPattern;

namespace CrossReview.Application.Project.UseCases.RemoveProjectMember;

public class RemoveProjectMemberUseCase 
{
    private readonly ILogger<RemoveProjectMemberUseCase> _logger;
    private readonly IValidator<RemoveProjectMemberRequest> _validator;
    private readonly IProjectRepository _repository;
    
    public RemoveProjectMemberUseCase(
        ILogger<RemoveProjectMemberUseCase> logger, 
        IValidator<RemoveProjectMemberRequest> validator, 
        IProjectRepository repository)
    {
        _logger = logger;
        _validator = validator;
        _repository = repository;
    }
    
    public async Task<Result<Guid, Errors>> Execute(RemoveProjectMemberRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return validationResult.ToList();

        var project = await _repository.GetByIdAsync(request.ProjectId);

        if (project is null)
            return GeneralErrors.NotFound(request.ProjectId).ToErrors();

        var result = project.RemoveEmployeeFromProject(request.UserId);
        
        if (result.IsFailure)
            return GeneralErrors.NotFound(request.UserId).ToErrors();

        await _repository.SaveAsync(cancellationToken);
        
        _logger.LogInformation("User with Id {request.UserId} in project with id {request.ProjectId} was removed",
            request.UserId, request.ProjectId);

        return project.Id;
    }
}