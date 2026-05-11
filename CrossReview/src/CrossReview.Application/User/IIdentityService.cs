using CrossReview.Application.User.DTO;
using CrossReview.Application.User.UseCases.Register;
using CrossReview.Application.User.UseCases.RegisterAdmin;

namespace CrossReview.Application.User;

public interface IIdentityService
{
    Task<UserDto?> GetById(Guid id);
    Task<UserDto?> GetByEmail(string email);

    Task<JwtUserModel?> Login(string email, string password);

    Task<bool> CheckPassword(string email, string password);

    Task<UserIdentityResult?> RegisterUser(RegisterUserRequest request);
    Task<UserIdentityResult?> RegisterAdmin(RegisterAdminRequest request);

    Task<bool> Delete(Guid id);
}