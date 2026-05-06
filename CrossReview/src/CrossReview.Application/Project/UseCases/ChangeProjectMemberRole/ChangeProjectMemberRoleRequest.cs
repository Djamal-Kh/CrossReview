using CrossReview.Domain.Project;

namespace CrossReview.Application.Project.UseCases.ChangeProjectMemberRole;

public record ChangeProjectMemberRoleRequest(Guid UserId, EnumProjectRole Role, Guid ProjectId);