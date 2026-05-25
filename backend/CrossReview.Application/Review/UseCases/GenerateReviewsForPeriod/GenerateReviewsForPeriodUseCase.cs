// GenerateReviewsForPeriodUseCase.cs

using CrossReview.Application.Project;
using CrossReview.Application.Review;
using CrossReview.Application.Review.UseCases.GenerateReviewsForPeriod;
using CrossReview.Application.Template;
using CrossReview.Domain.Project;
using CrossReview.Domain.Review;
using CSharpFunctionalExtensions;
using Shared.Common.ResultPattern;

public class GenerateReviewsForPeriodUseCase
{
    private readonly IProjectRepository _projectRepository;
    private readonly IReviewRepository _reviewRepository;
    private readonly ITemplateRepository _templateRepository;

    public GenerateReviewsForPeriodUseCase(
        IProjectRepository projectRepository,
        IReviewRepository reviewRepository,
        ITemplateRepository templateRepository)
    {
        _projectRepository = projectRepository;
        _reviewRepository = reviewRepository;
        _templateRepository = templateRepository;
    }

    public async Task<Result<int, Errors>> Execute(
        GenerateReviewsForPeriodRequest request,
        CancellationToken cancellationToken)
    {
        var project = await _projectRepository.GetByIdAsync(request.ProjectId, cancellationToken);

        if (project is null)
            return GeneralErrors.NotFound(request.ProjectId).ToErrors();

        // Проверяем что запрашивающий — Admin или TeamLead в этом проекте
        if (!request.IsAdmin)
        {
            var requester = project.Members
                .FirstOrDefault(m => m.UserId == request.RequestedByUserId && m.IsActive);

            if (requester is null || requester.Role != EnumProjectRole.TeamLead)
                return GeneralErrors.ValueIsInvalid("Недостаточно прав").ToErrors();
        }

        var period = project.ReviewPeriods.FirstOrDefault(p => p.Id == request.PeriodId);

        if (period is null)
            return GeneralErrors.NotFound(request.PeriodId).ToErrors();

        if (!period.IsActiveNow())
            return GeneralErrors.ValueIsInvalid("Период не активен").ToErrors();

        var template = await _templateRepository.GetByIdAsync(request.TemplateId, cancellationToken);

        if (template is null)
            return GeneralErrors.NotFound(request.TemplateId).ToErrors();

        if (template.ProjectId != project.Id)
            return GeneralErrors.ValueIsInvalid("Шаблон не принадлежит проекту").ToErrors();

        // Активные участники
        var activeMembers = project.Members
            .Where(m => m.IsActive)
            .ToList();

        if (activeMembers.Count < 2)
            return GeneralErrors.ValueIsInvalid("Недостаточно участников для ревью").ToErrors();

        int created = 0;

        // Все на всех (каждый оценивает каждого)
        foreach (var reviewer in activeMembers)
        {
            foreach (var reviewee in activeMembers)
            {
                if (reviewer.UserId == reviewee.UserId)
                    continue;

                // Проверяем что такое ревью ещё не существует
                var exists = await _reviewRepository.ExistsReviewAsync(
                    reviewer.UserId,
                    reviewee.UserId,
                    request.PeriodId,
                    cancellationToken);

                if (exists)
                    continue;

                var review = ReviewEntity.Create(
                    reviewer.UserId,
                    reviewee.UserId,
                    request.ProjectId,
                    request.TemplateId,
                    request.PeriodId);

                await _reviewRepository.AddAsync(review, cancellationToken);
                created++;
            }
        }

        return created;
    }
}