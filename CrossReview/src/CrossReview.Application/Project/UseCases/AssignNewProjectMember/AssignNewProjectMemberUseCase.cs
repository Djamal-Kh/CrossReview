using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Shared.Common.Extensions;
using Shared.Common.ResultPattern;

namespace CrossReview.Application.Project.UseCases.AssignNewProjectMember;

public class AssignNewProjectMemberUseCase
{
    private readonly ILogger<AssignNewProjectMemberUseCase> _logger;
    private readonly IProjectRepository _repository;
    private readonly IValidator<AssignNewProjectMemberRequest> _validator;

    public AssignNewProjectMemberUseCase(
        ILogger<AssignNewProjectMemberUseCase> logger, 
        IProjectRepository repository, 
        IValidator<AssignNewProjectMemberRequest> validator)
    {
        _logger = logger;
        _repository = repository;
        _validator = validator;
    }

    public async Task<Result<Guid, Errors>> Execute(AssignNewProjectMemberRequest request,CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return validationResult.ToList();

        var project = await _repository.GetByIdAsync(request.ProjectId);
        
        if (project is null)
            return GeneralErrors.NotFound(request.ProjectId).ToErrors();

        var memberId = project.AssignEmployeeToProject(null, request.Role);

        await _repository.SaveAsync(project, cancellationToken);
        
        _logger.LogInformation(
            "Employee with UserId {UserId} was assign to Project with Id {ProjectId} and his Role:{Role}",
            memberId,
            request.ProjectId,
            request.Role);
        
        return memberId;
    }
}