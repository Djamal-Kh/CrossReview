namespace CrossReview.Application.Project.UseCases.DeactivateProjectMember;

public record DeactivateProjectMemberRequest(Guid ProjectId, Guid UserId);