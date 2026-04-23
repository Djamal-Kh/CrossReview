namespace CrossReview.Application.Review.UseCases.UpdateAnswerInReview;

public record UpdateAnswerRequest(Guid ReviewId, Guid QuestionId, int Score, string Comment);