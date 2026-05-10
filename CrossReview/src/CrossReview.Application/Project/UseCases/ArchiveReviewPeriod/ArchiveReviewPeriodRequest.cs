namespace CrossReview.Application.Project.UseCases.ArchiveReviewPeriod;

public record ArchiveReviewPeriodRequest(Guid ProjectId, Guid ReviewPeriodId);