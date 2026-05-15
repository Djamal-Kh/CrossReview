using CrossReview.Application.Review;
using CrossReview.Domain.Review;

namespace CrossReview.Infrastructure.Postgres.Seeds;

public class ReviewSeed
{
    public static async Task SeedAsync(
        IReviewRepository repository,
        Guid projectId,
        Guid templateId,
        Guid reviewerId,
        Guid revieweeId,
        Guid periodId,
        CancellationToken ct)
    {
        var existing = await repository.GetAllAsyncForSeed(projectId, ct);

        if (existing.Any())
            return;

        var review = ReviewEntity.Create(
            reviewerId,
            revieweeId,
            projectId,
            templateId,
            periodId);

        await repository.AddAsync(review, ct);
        await repository.SaveAsync(ct);
    }
}