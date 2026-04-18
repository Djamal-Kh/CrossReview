using CrossReview.Domain.Project;

namespace CrossReview.Application.Project.UseCases.ChangeEmployeeRole;

public record ChangeProjectMemberRoleRequest(Guid UserId, EnumProjectRole Role, Guid ProjectId);