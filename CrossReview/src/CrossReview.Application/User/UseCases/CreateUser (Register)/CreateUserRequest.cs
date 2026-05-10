using CrossReview.Domain.User;

namespace CrossReview.Application.User.UseCases.CreateUser__Register_;

public record CreateUserRequest(string FirstName, string LastName, string Email, string Password, string PhoneNumber, EnumUserRole Role = EnumUserRole.User);