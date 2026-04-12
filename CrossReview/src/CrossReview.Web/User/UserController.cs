using Microsoft.AspNetCore.Mvc;

namespace CrossReview.User;

public class UserController : ControllerBase
{
    public UserController()
    {
        
    }

    [HttpPost]
    private async Task AddUser()
    {
        return;
    }

    [HttpGet("userId")]
    private async Task GetUser()
    {
        
    }

    [HttpGet]
    private async Task GetUsers()
    {
        
    }

    [HttpPut("userId")]
    private async Task ChangeUser()
    {
        
    }

    [HttpDelete("userId")]
    private async Task RemoveUser()
    {
        
    }
}