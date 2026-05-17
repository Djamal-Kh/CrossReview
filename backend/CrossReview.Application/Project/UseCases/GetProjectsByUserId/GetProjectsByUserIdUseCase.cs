using CrossReview.Application.Project.DTOs;
using CrossReview.Domain.Project;
using CSharpFunctionalExtensions;
using Shared.Common.ResultPattern;

namespace CrossReview.Application.Project.UseCases.GetProjectsByUserId;

public class GetProjectsByUserIdUseCase
{
    private readonly IProjectRepository _projectRepository;
    
    public GetProjectsByUserIdUseCase(IProjectRepository projectRepository)
    {
        _projectRepository = projectRepository;
    }

    public async Task<Result<List<ProjectListItemDto>, Errors>> Execute(GetProjectsByUserIdRequest request,
        CancellationToken cancellationToken)
    {
        var projects = await _projectRepository.GetAllAsyncById(request.UserId, cancellationToken);
        
        if (projects.Count == 0)
            return GeneralErrors.CollectionEmpty().ToErrors();

        var result = projects.Select(p => new ProjectListItemDto
        {
            Id = p.Id,
            Title =  p.Title,
            Status =  p.Status,
            MembersCount = p.Members.Count,
            ActiveReviewPeriods = p.ReviewPeriods
                .Count(rp => rp.Status == EnumReviewPeriodStatus.Active)
        }).ToList();
        
        return result;
    }
}