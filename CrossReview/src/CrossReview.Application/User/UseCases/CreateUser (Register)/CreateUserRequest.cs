using CrossReview.Domain.User;

namespace CrossReview.Application.User.UseCases.CreateUser;

public record CreateUserRequest(string FirstName, string LastName, string Email, string Password, string PhoneNumber, EnumUserRole Role = EnumUserRole.User);