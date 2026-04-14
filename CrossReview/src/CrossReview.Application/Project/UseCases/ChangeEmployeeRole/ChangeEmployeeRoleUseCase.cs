using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Shared.Common.Extensions;
using Shared.Common.ResultPattern;

namespace CrossReview.Application.Project.UseCases.ChangeEmployeeRole;

public class ChangeEmployeeRoleUseCase
{
    private readonly ILogger<ChangeEmployeeRoleUseCase> _logger;
    private readonly IProjectRepository _repository;
    private readonly IValidator<ChangeEmployeeRoleRequest> _validator;

    public ChangeEmployeeRoleUseCase(
        ILogger<ChangeEmployeeRoleUseCase> logger, 
        IProjectRepository repository, 
        IValidator<ChangeEmployeeRoleRequest> validator)
    {
        _logger = logger;
        _repository = repository;
        _validator = validator;
    }

    public async Task<Result<Guid, Errors>> Execute(ChangeEmployeeRoleRequest request,
        CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return validationResult.ToList();

        var project = await _repository.GetByIdAsync(request.ProjectId, cancellationToken);
        
        if (project is null)
            return GeneralErrors.NotFound(request.ProjectId).ToErrors();

        var employeeId = project.AssignEmployeeToProject(request.UserId, request.Role);

        await _repository.SaveAsync(project, cancellationToken);
        
        _logger.LogInformation(
            "Employee with UserId {UserId} was assign to Project with Id {ProjectId} and his Role: {Role}",
            employeeId,
            request.ProjectId,
            request.Role);
        
        return employeeId;
    }
}