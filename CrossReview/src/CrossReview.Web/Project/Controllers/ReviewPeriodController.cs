using CrossReview.Application.Project.UseCases.AddNewReviewPeriod;
using Microsoft.AspNetCore.Mvc;

namespace CrossReview.Project.Controllers;

[Route("api/project/review-period")]
[ApiController]
public class ReviewPeriodController : ControllerBase
{
    private readonly AddNewPeriodUseCase _addNewPeriodUseCase;
    
    public ReviewPeriodController(
        AddNewPeriodUseCase addNewPeriodUseCase)
    {
        _addNewPeriodUseCase = addNewPeriodUseCase;
    }
    
    [HttpPost]
    [Route("create")]
    public async Task<IActionResult> Create(
        Guid projectId,
        DateTime startDate,
        DateTime endDate,
        CancellationToken cancellationToken)
    {
        var request = new AddNewPeriodRequest(projectId, startDate, endDate);

        var result = await _addNewPeriodUseCase.Execute(request, cancellationToken);
        
        if (result.IsFailure)
            return BadRequest(result.Error);
        
        return Ok(result.Value);
    }
    
    // добавить UseCases: CloseReviewPeriod, ArchiveReviewPeriod, UpdateDates
}