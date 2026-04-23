using CrossReview.Application.Project;
using CrossReview.Application.Template;
using CrossReview.Domain.Review;
using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Shared.Common.ResultPattern;

namespace CrossReview.Application.Review.UseCases.CreateReview;

public class CreateReviewUseCase
{
    private readonly ILogger<CreateReviewUseCase> _logger;
    private readonly IReviewRepository _reviewRepository;
    private readonly IProjectRepository _projectRepository;
    private readonly ITemplateRepository _templateRepository;
    
    public CreateReviewUseCase(
        ILogger<CreateReviewUseCase> logger, 
        IReviewRepository reviewRepository, 
        IProjectRepository projectRepository, 
        ITemplateRepository templateRepository)
    {
        _logger = logger;
        _reviewRepository = reviewRepository;
        _projectRepository = projectRepository;
        _templateRepository = templateRepository;
    }

    public async Task<Result<Guid, Errors>> Execute(CreateReviewRequest request)
    {
        var project = await _projectRepository.GetByIdAsync(request.ProjectId);

        if (project is null)
            return GeneralErrors.NotFound(request.ProjectId).ToErrors();
        
        var period = project.ReviewPeriods.FirstOrDefault(p => p.Id == request.PeriodId);

        if (period is null)
            return GeneralErrors.NotFound(request.PeriodId).ToErrors();
        
        var isActive = period.IsActiveNow();

        if (!isActive)
            return GeneralErrors.ValueIsInvalid("Период не активен").ToErrors();
        
        project.EnsureUsersCanReviewEachOther(request.ReviewerId, request.RevieweeId);

        var template = await _templateRepository.GetByIdAsync(request.TemplateId);
        
        if (template is null)
            return GeneralErrors.NotFound(request.TemplateId).ToErrors();
        
        if (template.ProjectId != project.Id)
            return GeneralErrors.ValueIsInvalid("Шаблон не принадлежит проекту").ToErrors();
            
        project.EnsureTemplateBelongsToProject(template.ProjectId);
        
        var review = ReviewEntity.Create(
            request.ReviewerId,
            request.RevieweeId,
            request.ProjectId,
            request.TemplateId,
            request.PeriodId);

        var reviewExists = await _reviewRepository.ExistsReviewAsync(
            request.ReviewerId,
            request.RevieweeId,
            request.PeriodId);
        
        if (reviewExists)
            return GeneralErrors.ValueIsInvalid("Отзыв уже существует").ToErrors();
        
        await _reviewRepository.AddAsync(review);
        
        _logger.LogInformation("Review created {ReviewId}, reviewer {ReviewerId}, reviewee {RevieweeId}, period {PeriodId}",
            review.Id,
            request.ReviewerId,
            request.RevieweeId,
            request.PeriodId);

        return review.Id;
    }
}