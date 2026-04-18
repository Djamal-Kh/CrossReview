namespace CrossReview.Application.Project.UseCases.DeactivateEmployee;

public record DeactivateProjectMemberRequest(Guid ProjectId, Guid UserId);