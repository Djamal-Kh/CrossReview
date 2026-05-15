using CrossReview.Application.User;
using CrossReview.Application.User.DTO;
using CrossReview.Application.User.UseCases.Register;
using CrossReview.Application.User.UseCases.RegisterAdmin;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Identity;
using Shared.Common.ResultPattern;

namespace Crossreview.Infrastructure.Identity;

public class IdentityService : IIdentityService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IJwtProvider _jwtProvider;

    public IdentityService(UserManager<AppUser> userManager, IJwtProvider jwtProvider)
    {
        _userManager = userManager;
        _jwtProvider = jwtProvider;
    }


    public async Task<UserDto?> GetById(Guid id)
    {
        var user = await _userManager.FindByIdAsync(id.ToString());
        if (user is null) return null;

        return Map(user);
    }

    public async Task<UserDto?> GetByEmail(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user is null) return null;

        return Map(user);
    }

    public async Task<bool> CheckPassword(string email, string password)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user is null) return false;

        return await _userManager.CheckPasswordAsync(user, password);
    }

    public async Task<Result<UserIdentityResult?, Errors>> RegisterUser(RegisterUserRequest request)
    {
        var user = new AppUser
        {
            UserName = request.Email,
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName
        };

        var result = await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
            return GeneralErrors.DuplicateValues(result.Errors.ToString()).ToErrors();
        
        await _userManager.AddToRoleAsync(user, "User");

        var roles = await _userManager.GetRolesAsync(user);

        return new UserIdentityResult
        {
            Id = user.Id,
            Email = user.Email!,
            Roles = roles.ToList()
        };
    }

    public async Task<Result<UserIdentityResult?, Errors>> RegisterAdmin(RegisterAdminRequest request)
    {
        var user = new AppUser
        {
            UserName = request.Email,
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName
        };

        var result = await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
            return GeneralErrors.DuplicateValues(result.Errors.ToString()).ToErrors();
        
        await _userManager.AddToRoleAsync(user, "Admin");

        var roles = await _userManager.GetRolesAsync(user);

        return new UserIdentityResult
        {
            Id = user.Id,
            Email = user.Email!,
            Roles = roles.ToList()
        };
    }

    public async Task<bool> Delete(Guid id)
    {
        var user = await _userManager.FindByIdAsync(id.ToString());
        if (user is null) return false;

        var result = await _userManager.DeleteAsync(user);
        return result.Succeeded;
    }

    public async Task<JwtUserModel?> Login(string email, string password)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user is null)
            return null;

        var ok = await _userManager.CheckPasswordAsync(user, password);
        if (!ok)
            return null;

        var roles = await _userManager.GetRolesAsync(user);

        return new JwtUserModel
        {
            Id = user.Id,
            Email = user.Email!,
            Roles = roles.ToList()
        };
    }

    private static UserDto Map(AppUser user)
    {
        return new UserDto
        {
            Id = user.Id,
            Email = user.Email!,
            FirstName = user.FirstName,
            LastName = user.LastName
        };
    }
}