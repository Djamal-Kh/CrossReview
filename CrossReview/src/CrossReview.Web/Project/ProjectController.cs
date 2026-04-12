using Microsoft.AspNetCore.Mvc;

namespace CrossReview.Project;

public class ProjectController : ControllerBase
{
    public ProjectController()
    {
        
    }

    [HttpPost]
    public async Task Create()
    {
        
    }

    [HttpGet("projectId")]
    public async Task Get()
    {
        
    }

    [HttpPut("projectId")]
    public async Task Change()
    {
        
    }

    [HttpPatch("projectId/projectMemberid")]
    public async Task AddProjectMember()
    {
        
    }
    
    [HttpDelete("projectId")]
    public async Task Delete()
    {
        
    }
}