namespace CrossReview.Application.Project.UseCases.AddNewReviewPeriod;

public record AddNewPeriodRequest(Guid ProjectId, DateTime StartDate, DateTime EndDate);