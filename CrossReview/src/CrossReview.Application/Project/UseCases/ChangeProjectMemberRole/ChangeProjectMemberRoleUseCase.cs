using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Shared.Common.Extensions;
using Shared.Common.ResultPattern;

namespace CrossReview.Application.Project.UseCases.ChangeEmployeeRole;

public class ChangeProjectMemberRoleUseCase
{
    private readonly ILogger<ChangeProjectMemberRoleUseCase> _logger;
    private readonly IProjectRepository _repository;
    private readonly IValidator<ChangeProjectMemberRoleRequest> _validator;

    public ChangeProjectMemberRoleUseCase(
        ILogger<ChangeProjectMemberRoleUseCase> logger, 
        IProjectRepository repository, 
        IValidator<ChangeProjectMemberRoleRequest> validator)
    {
        _logger = logger;
        _repository = repository;
        _validator = validator;
    }

    public async Task<Result<Guid, Errors>> Execute(ChangeProjectMemberRoleRequest request,
        CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return validationResult.ToList();

        var project = await _repository.GetByIdAsync(request.ProjectId, cancellationToken);
        
        if (project is null)
            return GeneralErrors.NotFound(request.ProjectId).ToErrors();
        
        var result = project.ChangeEmployeeRole(request.UserId, request.Role);

        if (result.IsFailure)
            return result.Error;
        
        await _repository.SaveAsync(project, cancellationToken);
        
        _logger.LogInformation(
            "Employee with UserId {UserId} was assign to Project with Id {ProjectId} and his Role: {Role}",
            request.UserId,
            request.ProjectId,
            request.Role);
        
        return request.UserId;
    }
}