using CrossReview.Domain.Project;

namespace CrossReview.Application.Project.UseCases.AssignNewProjectMember;

public record AssignNewProjectMemberRequest(Guid UserId, EnumProjectRole Role, Guid ProjectId, Guid RequestedByUserId, bool IsAdmin);