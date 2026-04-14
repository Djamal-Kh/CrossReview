using CrossReview.Domain.Project;

namespace CrossReview.Application.Project.UseCases.ChangeEmployeeRole;

public record ChangeEmployeeRoleRequest(Guid UserId, EnumProjectRole Role, Guid ProjectId);