using CrossReview.Application.Project.UseCases.ActivateReviewPeriod;
using CrossReview.Application.Project.UseCases.AddNewReviewPeriod;
using CrossReview.Application.Project.UseCases.ArchiveReviewPeriod;
using CrossReview.Application.Project.UseCases.CloseReviewPeriod;
using CrossReview.Application.Project.UseCases.UpdateReviewPeriodDates;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CrossReview.Project.Controllers;

[Route("api/project/review-period")]
[ApiController]
public class ReviewPeriodController : ControllerBase
{
    private readonly AddNewPeriodUseCase _addNewPeriodUseCase;
    private readonly CloseReviewPeriodUseCase _closeReviewPeriodUseCase;
    private readonly ArchiveReviewPeriodUseCase _archiveReviewPeriodUseCase;
    private readonly UpdateReviewPeriodDatesUseCase _updateReviewPeriodDatesUseCase;
    private readonly ActivateReviewPeriodUseCase _activateReviewPeriodUseCase;
    
    public ReviewPeriodController(
        AddNewPeriodUseCase addNewPeriodUseCase, 
        CloseReviewPeriodUseCase closeReviewPeriodUseCase, 
        ArchiveReviewPeriodUseCase archiveReviewPeriodUseCase, 
        UpdateReviewPeriodDatesUseCase updateReviewPeriodDatesUseCase, 
        ActivateReviewPeriodUseCase activateReviewPeriodUseCase)
    {
        _addNewPeriodUseCase = addNewPeriodUseCase;
        _closeReviewPeriodUseCase = closeReviewPeriodUseCase;
        _archiveReviewPeriodUseCase = archiveReviewPeriodUseCase;
        _updateReviewPeriodDatesUseCase = updateReviewPeriodDatesUseCase;
        _activateReviewPeriodUseCase = activateReviewPeriodUseCase;
    }
    
    [HttpPost]
    [Route("create")]
    [Authorize(Roles = "Admin")]
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

    [HttpPatch]
    [Route("close")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Close(
        Guid projectId,
        Guid periodId,
        CancellationToken cancellationToken)
    {
        var request = new CloseReviewPeriodRequest(projectId, periodId);
        
        var result = await _closeReviewPeriodUseCase.Execute(request, cancellationToken);
        
        if (result.IsFailure)
            return BadRequest(result.Error);

        return Ok("Success");
    }
    
    [HttpPatch]
    [Route("archive")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Archive(
        Guid projectId,
        Guid periodId,
        CancellationToken cancellationToken)
    {
        var request = new ArchiveReviewPeriodRequest(projectId, periodId);
        
        var result = await _archiveReviewPeriodUseCase.Execute(request, cancellationToken);
        
        if (result.IsFailure)
            return BadRequest(result.Error);

        return Ok("Success");
    }
    
    [HttpPatch]
    [Route("update")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateDates(
        Guid projectId,
        Guid periodId,
        DateTime startDate,
        DateTime endDate,
        CancellationToken cancellationToken)
    {
        var request = new UpdateReviewPeriodDatesRequest(projectId, periodId, startDate, endDate);
        
        var result = await _updateReviewPeriodDatesUseCase.Execute(request, cancellationToken);
        
        if (result.IsFailure)
            return BadRequest(result.Error);

        return Ok("Success");
    }

    [HttpPatch]
    [Route("activate")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Activate(
        Guid projectId,
        Guid periodId,
        CancellationToken cancellationToken)
    {
        var request = new ActivateReviewPeriodRequest(projectId, periodId);

        var result = await _activateReviewPeriodUseCase.Execute(request, cancellationToken);
        
        if (result.IsFailure)
            return BadRequest(result.Error);

        return Ok("Success");
    }
}