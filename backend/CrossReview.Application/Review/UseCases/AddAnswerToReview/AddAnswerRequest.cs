namespace CrossReview.Application.Review.UseCases.AddAnswerToReview;

public record AddAnswerRequest(
    Guid ReviewId,
    Guid QuestionId,
    int Score,
    string? Comment);