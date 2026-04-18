using CrossReview.Domain.Project;

namespace CrossReview.Application.Project.UseCases.AssignEmployee;

public record AssignNewProjectMemberRequest(Guid UserId, EnumProjectRole Role, Guid ProjectId);