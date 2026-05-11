using CrossReview.Application.User.DTO;
using CrossReview.Application.User.UseCases.Register;
using CrossReview.Application.User.UseCases.RegisterAdmin;
using CSharpFunctionalExtensions;
using Shared.Common.ResultPattern;

namespace CrossReview.Application.User;

public interface IIdentityService
{
    Task<UserDto?> GetById(Guid id);
    Task<UserDto?> GetByEmail(string email);

    Task<JwtUserModel?> Login(string email, string password);

    Task<bool> CheckPassword(string email, string password);

    Task<Result<UserIdentityResult?, Errors>> RegisterUser(RegisterUserRequest request);
    Task<Result<UserIdentityResult?, Errors>> RegisterAdmin(RegisterAdminRequest request);

    Task<bool> Delete(Guid id);
}