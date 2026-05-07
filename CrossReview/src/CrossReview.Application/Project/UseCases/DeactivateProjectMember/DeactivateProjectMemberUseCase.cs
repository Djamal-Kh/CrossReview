using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Shared.Common.Extensions;
using Shared.Common.ResultPattern;

namespace CrossReview.Application.Project.UseCases.DeactivateProjectMember;

public class DeactivateProjectMemberUseCase
{
    private readonly ILogger<DeactivateProjectMemberUseCase> _logger;
    private readonly IProjectRepository _repository;
    private readonly IValidator<DeactivateProjectMemberRequest> _validator;
    
    public DeactivateProjectMemberUseCase(
        ILogger<DeactivateProjectMemberUseCase> logger, 
        IProjectRepository repository, 
        IValidator<DeactivateProjectMemberRequest> validator)
    {
        _logger = logger;
        _repository = repository;
        _validator = validator;
    }

    public async Task<Result<Guid, Errors>> Execute(DeactivateProjectMemberRequest request,
        CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return validationResult.ToList();

        var project = await _repository.GetByIdAsync(request.ProjectId);

        if (project is null)
            return GeneralErrors.NotFound(request.ProjectId).ToErrors();

        var result = project.DeactivateEmployeeInProject(request.UserId);

        if (result.IsFailure)
            return GeneralErrors.NotFound(request.UserId).ToErrors();

        await _repository.SaveAsync(cancellationToken);
        
        _logger.LogInformation("User with Id {request.UserId} in project with id {request.ProjectId} was deactivated",
            request.UserId, request.ProjectId);

        return project.Id;
    }
}