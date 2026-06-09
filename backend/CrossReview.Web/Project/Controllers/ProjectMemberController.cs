using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using CrossReview.Application.Project.UseCases.AssignNewProjectMember;
using CrossReview.Application.Project.UseCases.ChangeProjectMemberRole;
using CrossReview.Application.Project.UseCases.DeactivateProjectMember;
using CrossReview.Application.Project.UseCases.GetProjectMemberById;
using CrossReview.Application.Project.UseCases.GetProjectMembers;
using CrossReview.Application.Project.UseCases.RemoveProjectMember;
using CrossReview.Domain.Project;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CrossReview.Project.Controllers;

[Route("api/project/members/")]
[ApiController]
public class ProjectMemberController : ControllerBase
{
    private readonly AssignNewProjectMemberUseCase _assignNewProjectMemberUseCase;
    private readonly ChangeProjectMemberRoleUseCase _changeProjectMemberRoleUseCase;
    private readonly RemoveProjectMemberUseCase _removeProjectMemberUseCase;
    private readonly DeactivateProjectMemberUseCase _deactivateProjectMemberUseCase;
    private readonly GetProjectMemberByIdUseCase _getProjectMemberByIdUseCase;
    private readonly GetProjectMembersUseCase _getProjectMembersUseCase;
    
    public ProjectMemberController(
        AssignNewProjectMemberUseCase assignNewProjectMemberUseCase, 
        ChangeProjectMemberRoleUseCase changeProjectMemberRoleUseCase,
        RemoveProjectMemberUseCase removeProjectMemberUseCase,
        DeactivateProjectMemberUseCase deactivateProjectMemberUseCase,
        GetProjectMemberByIdUseCase getProjectMemberByIdUseCase, 
        GetProjectMembersUseCase getProjectMembersUseCase)
    {
        _assignNewProjectMemberUseCase = assignNewProjectMemberUseCase;
        _changeProjectMemberRoleUseCase = changeProjectMemberRoleUseCase;
        _removeProjectMemberUseCase = removeProjectMemberUseCase;
        _deactivateProjectMemberUseCase = deactivateProjectMemberUseCase;
        _getProjectMemberByIdUseCase = getProjectMemberByIdUseCase;
        _getProjectMembersUseCase = getProjectMembersUseCase;
    }

    [HttpPost]
    [Route("add")]
    [Authorize]
    public async Task<IActionResult> Add(
        Guid userId,
        EnumProjectRole role,
        Guid projectId,
        CancellationToken cancellationToken)
    {
        var currentUserId = GetCurrentUserId();
    
        if (currentUserId is null)
            return Unauthorized();

        var isAdmin = User.IsInRole("Admin");
        
        var request = new AssignNewProjectMemberRequest(userId, role, projectId, currentUserId.Value, isAdmin);

        var result = await _assignNewProjectMemberUseCase.Execute(request, cancellationToken);
        
        if (result.IsFailure)
            return BadRequest(result.Error);
        
        return Ok(result.Value);
    }
    
    [HttpGet]
    [Route("project/{projectId:guid}")]
    [Authorize]
    public async Task<IActionResult> GetProjectMembers(
        Guid projectId,
        CancellationToken cancellationToken)
    {
        var request = new GetProjectMembersRequest(projectId);
        
        var result = await _getProjectMembersUseCase.Execute(request, cancellationToken);
        
        if (result.IsFailure)
            return BadRequest(result.Error);
        
        return Ok(result.Value);
    }

    [HttpGet]
    [Route("by-id")]
    [Authorize]
    public async Task<IActionResult> GetProjectMemberById(
        [FromRoute] Guid projectId,
        [FromRoute] Guid userId,
        CancellationToken cancellationToken)
    {
        var request = new GetProjectMemberByIdRequest(projectId, userId);
        
        var result = await _getProjectMemberByIdUseCase.Execute(request, cancellationToken);
        
        if (result.IsFailure)
            return BadRequest(result.Error);
        
        return Ok(result.Value);
    }
    
    [HttpPatch]
    [Route("update-role")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateRole(
        Guid userId,
        EnumProjectRole role,
        Guid projectId,
        CancellationToken cancellationToken)
    {
        var request = new ChangeProjectMemberRoleRequest(userId, role, projectId);

        var result = await _changeProjectMemberRoleUseCase.Execute(request, cancellationToken);
        
        if (result.IsFailure)
            return BadRequest(result.Error);
        
        return Ok(result.Value);
    }

    [HttpPatch]
    [Route("deactivate-member")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Deactivate(
        Guid projectId,
        Guid userId,
        CancellationToken cancellationToken)
    {
        var request = new DeactivateProjectMemberRequest(projectId, userId);

        var result = await _deactivateProjectMemberUseCase.Execute(request, cancellationToken);
        
        if (result.IsFailure)
            return BadRequest(result.Error);
        
        return Ok(result.Value);
    }

    [HttpDelete]
    [Route("remove-member")]
    [Authorize]
    public async Task<IActionResult> Remove(
        Guid projectId,
        Guid userId,
        CancellationToken cancellationToken)
    {
        var currentUserId = GetCurrentUserId();
    
        if (currentUserId is null)
            return Unauthorized();

        var isAdmin = User.IsInRole("Admin");
        
        var request = new RemoveProjectMemberRequest(projectId, userId, currentUserId.Value, isAdmin);

        var result = await _removeProjectMemberUseCase.Execute(request, cancellationToken);

        if (result.IsFailure)
            return BadRequest(result.Error);
        
        return Ok(result.Value);
    }
    
    private Guid? GetCurrentUserId()
    {
        var sub = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value
                  ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    
        return Guid.TryParse(sub, out var id) ? id : null;
    }
}