using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Shared.Common.Extensions;
using Shared.Common.ResultPattern;

namespace CrossReview.Application.Project.UseCases.AddNewReviewPeriod;

public class AddNewPeriodUseCase
{
    private readonly ILogger<AddNewPeriodUseCase> _logger;
    private readonly IProjectRepository _repository;
    private readonly IValidator<AddNewPeriodRequest> _validator;
    
    public AddNewPeriodUseCase(
        ILogger<AddNewPeriodUseCase> logger, 
        IProjectRepository repository, 
        IValidator<AddNewPeriodRequest> validator)
    {
        _logger = logger;
        _repository = repository;
        _validator = validator;
    }

    public async Task<Result<Guid, Errors>> Execute(AddNewPeriodRequest request, CancellationToken cancellationToken)
    {
        // Оставляю пока без валидации
        
        //var validationResult = await _validator.ValidateAsync(request, cancellationToken);

        //if (!validationResult.IsValid)
          //  return validationResult.ToList();
        
        var project = await _repository.GetByIdAsync(request.ProjectId, cancellationToken);
        
        if (project is null)
            return GeneralErrors.NotFound(request.ProjectId).ToErrors();
        
        var periodId = project.CreateReviewPeriod(request.StartDate, request.EndDate);
        
        await _repository.SaveAsync(cancellationToken);

        _logger.LogInformation(
            "ReviewPeriod {PeriodId} created for project {ProjectId}",
            periodId,
            request.ProjectId);
        
        return periodId;
    }
}