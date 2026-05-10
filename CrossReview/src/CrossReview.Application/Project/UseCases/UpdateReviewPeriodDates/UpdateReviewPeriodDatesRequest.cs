namespace CrossReview.Application.Project.UseCases.UpdateReviewPeriodDates;

public record UpdateReviewPeriodDatesRequest(Guid ProjectId, Guid ReviewPeriodId, DateTime StartDate, DateTime EndDate);