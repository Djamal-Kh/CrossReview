namespace CrossReview.Application.Project.UseCases.ActivateReviewPeriod;

public record ActivateReviewPeriodRequest(Guid ProjectId, Guid PeriodId);