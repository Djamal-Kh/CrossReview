namespace CrossReview.Application.Project.UseCases.CloseReviewPeriod;

public record CloseReviewPeriodRequest(Guid ProjectId,Guid ReviewPeriodId);