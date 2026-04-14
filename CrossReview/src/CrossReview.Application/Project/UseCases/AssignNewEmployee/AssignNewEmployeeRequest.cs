using CrossReview.Domain.Project;

namespace CrossReview.Application.Project.UseCases.AssignEmployee;

public record AssignNewEmployeeRequest(Guid UserId, EnumProjectRole Role, Guid ProjectId);