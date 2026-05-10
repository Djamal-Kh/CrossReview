using CrossReview.Application.Project.UseCases.CloseProject;
using CrossReview.Application.Project.UseCases.CreateProject;
using CrossReview.Application.Project.UseCases.GetProjectById;
using CrossReview.Application.Project.UseCases.StartProject;
using CrossReview.Application.Project.UseCases.UpdateProjectData;
using CrossReview.Application.Project.UseCases.UpdateProjectDescription;
using CrossReview.Application.Project.UseCases.UpdateProjectTitle;
using Microsoft.AspNetCore.Mvc;

namespace CrossReview.Project.Controllers;

[Route("api/project/")]
[ApiController]
public class ProjectController : ControllerBase
{
    private readonly CloseProjectUseCase _closeProjectUseCase;
    private readonly CreateProjectUseCase _createProjectUseCase;
    private readonly GetProjectByIdUseCase _getProjectByIdUseCase;
    private readonly StartProjectUseCase _startProjectUseCase;
    private readonly UpdateProjectUseCase _updateProjectUseCase;
    private readonly UpdateProjectDescriptionUseCase _updateProjectDescriptionUseCase;
    private readonly UpdateProjectTitleUseCase _updateProjectTitleUseCase;
    
    public ProjectController(
        CloseProjectUseCase closeProjectUseCase, 
        CreateProjectUseCase createProjectUseCase, 
        GetProjectByIdUseCase getProjectByIdUseCase, 
        StartProjectUseCase startProjectUseCase, 
        UpdateProjectUseCase updateProjectUseCase, 
        UpdateProjectDescriptionUseCase updateProjectDescriptionUseCase, 
        UpdateProjectTitleUseCase updateProjectTitleUseCase)
    {
        _closeProjectUseCase = closeProjectUseCase;
        _createProjectUseCase = createProjectUseCase;
        _getProjectByIdUseCase = getProjectByIdUseCase;
        _startProjectUseCase = startProjectUseCase;
        _updateProjectUseCase = updateProjectUseCase;
        _updateProjectDescriptionUseCase = updateProjectDescriptionUseCase;
        _updateProjectTitleUseCase = updateProjectTitleUseCase;
    }

    [HttpPost]
    [Route("create")]
    public async Task<IActionResult> Create(
        string title, 
        string description, 
        CancellationToken cancellationToken)
    {
        var request = new CreateProjectRequest(title, description);
        
        var result = await _createProjectUseCase.Execute(request, cancellationToken);
        
        if (result.IsFailure)
            return BadRequest(result.Error);
        
        return Ok(result.Value);
    }

    [HttpGet]
    [Route("{id:guid}")]
    public async Task<IActionResult> Get(
        Guid id, 
        CancellationToken cancellationToken)
    {
        var request = new GetProjectByIdRequest(id);
        
        var result = await _getProjectByIdUseCase.Execute(request, cancellationToken);
        
        if (result.IsFailure)
            return BadRequest(result.Error);
        
        return Ok(result.Value);
    }

    // подумай над GetProjectUseCase
    [HttpGet]
    [Route("all")]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        return StatusCode(500);
    }

    [HttpPatch]
    [Route("{id:guid}/start")]
    public async Task<IActionResult> Start(
        Guid id,
        CancellationToken cancellationToken)
    {
        var request = new StartProjectRequest(id);
        
        var result = await _startProjectUseCase.Execute(request, cancellationToken);
        
        if (result.IsFailure)
            return BadRequest(result.Error);
        
        return Ok(result.Value);
    }

    [HttpPut]
    [Route("{id:guid}/update)")]
    public async Task<IActionResult> Update(
        Guid id,
        string title,
        string description,
        CancellationToken cancellationToken)
    {
        var request = new UpdateProjectRequest(id, title, description);
        
        var result = await _updateProjectUseCase.Execute(request, cancellationToken);
        
        if (result.IsFailure)
            return BadRequest(result.Error);
        
        return Ok(result.Value);
    }

    [HttpPatch]
    [Route("{id:guid}/update/description")]
    public async Task<IActionResult> UpdateDescription(
        Guid id,
        string description,
        CancellationToken cancellationToken)
    {
        var request = new UpdateProjectDescriptionRequest(id, description);
        
        var result = await _updateProjectDescriptionUseCase.Execute(request, cancellationToken);

        if (result.IsFailure)
            return BadRequest(result.Error);
        
        return Ok(result.Value);
    }

    [HttpPatch]
    [Route("{id:guid}/update/title")]
    public async Task<IActionResult> UpdateTitle(
        Guid id,
        string title,
        CancellationToken cancellationToken)
    {
        var request = new UpdateProjectTitleRequest(id, title);
        
        var result = await _updateProjectTitleUseCase.Execute(request, cancellationToken);
        
        if (result.IsFailure)
            return BadRequest(result.Error);
        
        return Ok(result.Value);
    }
    
    [HttpDelete]
    [Route("{id:guid}/close")]
    public async Task<IActionResult> Close(
        Guid id,
        CancellationToken cancellationToken)
    {
        var request = new CloseProjectRequest(id);
        
        var result = await _closeProjectUseCase.Execute(request, cancellationToken);

        if (result.IsFailure)
            return BadRequest(result.Error);
        
        return Ok(result.Value);
    }
}