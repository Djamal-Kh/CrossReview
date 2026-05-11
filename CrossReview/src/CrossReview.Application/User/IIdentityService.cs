using CrossReview.Application.User.DTO;
using CrossReview.Application.User.UseCases.CreateUser__Register_;

namespace CrossReview.Application.User;

public interface IIdentityService
{
    Task<UserDto?> GetById(Guid id);
    Task<UserDto?> GetByEmail(string email);

    Task<JwtUserModel?> Login(string email, string password);

    Task<bool> CheckPassword(string email, string password);

    Task<UserIdentityResult?> Register(RegisterUserRequest request);

    Task<bool> Delete(Guid id);
}